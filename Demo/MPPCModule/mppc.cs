using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPPCmodule;
using Windows.UI.Popups;

namespace Demo.MPPCModule
{
    public class Mppc
    {
        public static IntPtr DeviceHandle = mppcum1a.INVALID_HANDLE_VALUE;

        public static IntPtr PipeHandle = mppcum1a.INVALID_HANDLE_VALUE;

        public static uint ParamGateTime = 1;		// 1ms

        public static ushort ParamDataSize = 100;	// 1ms * 100 = 100ms

        private const uint CYCLE_TIME = 100;

        public static async void MPPC_Init(uint GateTime, ushort Threshold)
        {
            ushort result;

            ParamGateTime = GateTime;

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

            ParamDataSize = Convert.ToUInt16(CYCLE_TIME / GateTime);
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
                await Task.Delay(1000);
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
                if(result == mppcum1a.MPPC_NOT_UPDATED)
                {
                    Task.Delay(20);
                    continue;
                }
                else if(result == mppcum1a.MPPC_SUCCESS)
                {
                    for (i = 0; i < ParamDataSize; i++)
                    {
                        sumData += Convert.ToDouble(dat[i]);
                    }
                    
                    return sumData / ParamDataSize / ParamGateTime;
                }
            }
        }
    }
}
