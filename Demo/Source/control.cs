using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace Demo.Source
{
    public class Control
    {
        private SerialDevice SerialPort;
        private int ParamXStep = 0;
        private int ParamYStep = 0;
        private int XSpeed = 5000;
        private int YSpeed = 5000;
        private int RowN = 0;
        private int ColN = 0;
        private string isContinue = "false";
        private volatile bool isStop = true;
        private string PortName = "";

        public async void DeviceInit()
        {
            ParamsLoad();
            await SerialInitAsync();
            await MotorConfigAsync();
            MPPC.MPPC_Init();
        }

        public void ParamsLoad()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            XSpeed = Convert.ToInt32(localSettings.Values["XSpeed"].ToString());
            YSpeed = Convert.ToInt32(localSettings.Values["YSpeed"].ToString());
            int PicXLen = Convert.ToInt32(localSettings.Values["PicXLen"].ToString());
            int PicYLen = Convert.ToInt32(localSettings.Values["PicYLen"].ToString());
            int PicXCount = Convert.ToInt32(localSettings.Values["PicXCount"].ToString());
            int PicYCount = Convert.ToInt32(localSettings.Values["PicYCount"].ToString());

            ParamXStep = Convert.ToInt32(PicXLen * 400.0 / (PicXCount - 1));
            ParamYStep = Convert.ToInt32(PicYLen * 400.0 / (PicYCount - 1));

            PortName = localSettings.Values["PortName"].ToString();

            if (localSettings.Values["isContinue"].ToString() == "true")
            {
                isContinue = localSettings.Values["isContinue"].ToString();
                RowN = Convert.ToInt32(localSettings.Values["RowN"].ToString());
                ColN = Convert.ToInt32(localSettings.Values["ColN"].ToString());
            }

        }

        public async Task SerialInitAsync()
        {
            string filter = SerialDevice.GetDeviceSelector();
            var dis = await DeviceInformation.FindAllAsync(filter);
            DeviceInformation ID = null;
            foreach (var id in dis)
            {
                if (id.Name.ToString() == PortName)
                {
                    ID = id;
                    break;
                }
            }

            SerialPort = await SerialDevice.FromIdAsync(ID.Id);

            if (null != SerialPort)
            {
                /* Configure serial settings */
                SerialPort.WriteTimeout = TimeSpan.FromMilliseconds(20);
                SerialPort.ReadTimeout = TimeSpan.FromMilliseconds(20);
                SerialPort.BaudRate = 115200;
                SerialPort.Parity = SerialParity.None;
                SerialPort.StopBits = SerialStopBitCount.One;
                SerialPort.DataBits = 8;

                await new MessageDialog("SerialDevice Opened! ").ShowAsync();
            }
            else
            {
                await new MessageDialog("SerialDevice Not Found! ").ShowAsync();
                return;
            }

        }

        public void SerialClose()
        {
            SerialPort.Dispose();
        }

        private async void WaitStopAsync()
        {
            isStop = false;
            while (!isStop)
            {
                Task.Delay(TimeSpan.FromMilliseconds(20)).Wait();
                await SerialPortReadAsync();
            }

        }

        public async Task MotorConfigAsync()
        {

            await SerialPortWriteAsync("VX=" + XSpeed.ToString() + "/");
            await SerialPortWriteAsync("VY=" + YSpeed.ToString() + "/");
            await SerialPortWriteAsync("HVX=8000/");
            await SerialPortWriteAsync("HVY=8000/");
            await SerialPortWriteAsync("-HX/");
            await SerialPortWriteAsync("-HY/");
            if (isContinue == "true")
            {
                await SerialPortWriteAsync("X:" + (ParamXStep * ColN).ToString() + "/");
                await SerialPortWriteAsync("Y:" + (ParamYStep * RowN).ToString() + "/");
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

        public async Task XOneStepAsync(bool isForward)
        {
            if (isForward)
                await SerialPortWriteAsync("X:" + ParamXStep.ToString() + "/");
            else
                await SerialPortWriteAsync("X:-" + ParamXStep.ToString() + "/");
            WaitStopAsync();
        }

        public async Task YOneStepAsync()
        {
            await SerialPortWriteAsync("X:" + ParamYStep.ToString() + "/");
            WaitStopAsync();
        }

        private async Task SerialPortWriteAsync(string txBuffer)
        {
            using (DataWriter dataWriter = new DataWriter(SerialPort.OutputStream))
            {
                dataWriter.WriteString(txBuffer);
                await dataWriter.StoreAsync();
                dataWriter.DetachStream();
            }
        }

        private async Task SerialPortReadAsync()
        {
            const uint maxReadLength = 16;
            DataReader dataReader = new DataReader(SerialPort.InputStream)
            {
                InputStreamOptions = InputStreamOptions.Partial
            };
            Task<UInt32> loadAsyncTask = dataReader.LoadAsync(maxReadLength).AsTask();
            Task.Delay(TimeSpan.FromMilliseconds(20)).Wait();
            uint bytesRead = await loadAsyncTask;
            if (bytesRead > 0)
            {
                string rxBuffer = dataReader.ReadString(bytesRead);
                Debug.WriteLine(rxBuffer);
                if (-1 != rxBuffer.IndexOf("ST=00"))
                    isStop = true;
            }

            dataReader.DetachStream();
        }
    }
}
