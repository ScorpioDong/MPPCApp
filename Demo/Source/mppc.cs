using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MPPCmodule;
using Windows.Storage;
using Windows.UI.Popups;

namespace Demo.Source
{
    public static class MPPC
    {
        private static IntPtr DeviceHandle = mppcum1a.INVALID_HANDLE_VALUE;

        private static IntPtr PipeHandle = mppcum1a.INVALID_HANDLE_VALUE;

        private static readonly uint[] GateTime = new uint[7] { 1, 2, 5, 10, 20, 50, 100 };

        private static uint ParamGateTime = 1;		// 1ms

        private static ushort ParamDataSize = 100;	// 1ms * 100 = 100ms

        private const uint CYCLE_TIME = 100;

        public static async void MPPC_Init()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            ushort result;

            ParamGateTime = GateTime[Convert.ToUInt32(localSettings.Values["GateTime"].ToString())];
            ushort Threshold = Convert.ToUInt16(localSettings.Values["Threshold"].ToString());
            ParamDataSize = Convert.ToUInt16(CYCLE_TIME / ParamGateTime);

            DeviceHandle = mppcum1a.MPPC_OpenDevice();
            if(DeviceHandle == mppcum1a.INVALID_HANDLE_VALUE)
            {
                await new MessageDialog("Open Device Error！").ShowAsync();
                return;
            }

            PipeHandle = mppcum1a.MPPC_OpenPipe(DeviceHandle);
            if (PipeHandle == mppcum1a.INVALID_HANDLE_VALUE)
            {
                await new MessageDialog("Open Pipe Error！").ShowAsync();
                return;
            }

            result = mppcum1a.MPPC_Initialize(DeviceHandle);
            if(result != mppcum1a.MPPC_SUCCESS)
            {
                await new MessageDialog("Device Init Error！").ShowAsync();
                return;
            }
            
            result = mppcum1a.MPPC_SetProperty(DeviceHandle, ParamGateTime, ParamDataSize);
            if(result != mppcum1a.MPPC_SUCCESS)
            {
                await new MessageDialog("Set Property Error！").ShowAsync();
                return;
            }

            if (Threshold <= mppcum1a.VTH_75)
                result = mppcum1a.MPPC_SetThreshold(DeviceHandle, Threshold);
            else
                result = mppcum1a.MPPC_SetThreshold(DeviceHandle, mppcum1a.VTH_OVER);

            if(result != mppcum1a.MPPC_SUCCESS)
            {
                await new MessageDialog("Set Threshold Error！").ShowAsync();
                return;
            }

            result = mppcum1a.MPPC_GetPeltierControl(DeviceHandle, out ushort flag);
            if(result == mppcum1a.MPPC_SUCCESS)
            {
                if (flag == mppcum1a.ENABLE)
                    mppcum1a.MPPC_SetPeltierControl(DeviceHandle, mppcum1a.DISABLE);
            }
            else
            {
                await new MessageDialog("Get PeltierControl Error！").ShowAsync();
                return;
            }

            while(true)
            {
                mppcum1a.MPPC_SetPeltierControl(DeviceHandle, mppcum1a.ENABLE);
                result = mppcum1a.MPPC_GetPeltierStatus(DeviceHandle);
                if (result == mppcum1a.MPPC_SUCCESS)
                    return;
                Thread.Sleep(1000);
            }
        }

        public static double MPPC_GetCount()
        {
            ushort result;
            uint[] dat = new uint[ParamDataSize];
            double sumData = 0.0;
            int i;

            while(true)
            {
                result = mppcum1a.MPPC_GetCounterData(DeviceHandle, PipeHandle, ParamDataSize, dat);
                if (result == mppcum1a.MPPC_NOT_UPDATED)
                {
                    Thread.Sleep(20);
                    continue;
                }
                else if (result == mppcum1a.MPPC_SUCCESS)
                {
                    for (i = 0; i < ParamDataSize; i++)
                    {
                        sumData += Convert.ToDouble(dat[i]);
                    }

                    return sumData / ParamDataSize / ParamGateTime;
                }
                else
                    Debug.WriteLine("读取错误!");
            }
        }
    }
}
