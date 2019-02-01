// ------------------------------------------------------------------------
//  File        : mppcum1a.cs
//  Description	: Function definition file (.NET Framework 2.0 or later)
//
//  Copyright (C) Hamamatsu Photonics K.K. All Rights Reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MPPCmodule
{
    class mppcum1a
    {

        // Error handle 
        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        // Error code
        public const ushort MPPC_SUCCESS = (0);
        public const ushort MPPC_UNSUCCESS = (1);
        public const ushort MPPC_INVALID_HANDLE = (2);
        public const ushort MPPC_INVALID_VALUE = (3);
        public const ushort MPPC_NOT_UPDATED = (4);
        public const ushort MPPC_NON_SUPPORT = (5);
        public const ushort MPPC_UNSTABLE_STATE = (11);
        public const ushort MPPC_PELTIER_DISABLE = (12);
        public const ushort MPPC_PELTIER_TIMEOUT = (13);
        public const ushort MPPC_ERROR_MODULE = (14);
        public const ushort MPPC_ERROR_TEMP = (15);
        public const ushort MPPC_CHECK_NORMAL = (21);
        public const ushort MPPC_CHECK_INVALID = (22);
        public const ushort MPPC_CHECK_REMOVE = (23);

        // Peltier control
        public const ushort DISABLE = (0);
        public const ushort ENABLE = (1);

        // Threshold level
        public const ushort VTH_05 = (0);
        public const ushort VTH_15 = (1);
        public const ushort VTH_25 = (2);
        public const ushort VTH_35 = (3);
        public const ushort VTH_45 = (4);
        public const ushort VTH_55 = (5);
        public const ushort VTH_65 = (6);
        public const ushort VTH_75 = (7);
        public const ushort VTH_OVER = (40);

        // Functions
        [DllImport("mppcum1a.dll")]
        public extern static System.IntPtr MPPC_OpenDevice();
        [DllImport("mppcum1a.dll")]
        public extern static System.IntPtr MPPC_OpenTargetDevice(string UnitID);
        [DllImport("mppcum1a.dll")]
        public extern static void MPPC_CloseDevice(System.IntPtr DeviceHandle);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_CheckDevice(System.IntPtr DeviceHandle);
        [DllImport("mppcum1a.dll")]
        public extern static System.IntPtr MPPC_OpenPipe(System.IntPtr DeviceHandle);
        [DllImport("mppcum1a.dll")]
        public extern static void MPPC_ClosePipe(System.IntPtr PipeHandle);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_ReadTypeNumber(System.IntPtr DeviceHandle, System.Text.StringBuilder TypeNumber);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_ReadUnitID(System.IntPtr DeviceHandle, System.Text.StringBuilder UnitID);

        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_Initialize(System.IntPtr DeviceHandle);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_GetProperty(System.IntPtr DeviceHandle, out uint GateTime, out ushort DataSize);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_SetProperty(System.IntPtr DeviceHandle, uint GateTime, ushort DataSize);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_GetThreshold(System.IntPtr DeviceHandle, out ushort Level);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_SetThreshold(System.IntPtr DeviceHandle, ushort Level);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_GetThresholdVoltage(System.IntPtr DeviceHandle, out ushort Voltage);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_SetThresholdVoltage(System.IntPtr DeviceHandle, ushort Voltage);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_GetTemperature(System.IntPtr DeviceHandle, out double Temperature);

        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_GetPeltierControl(System.IntPtr DeviceHandle, out ushort ControlFlag);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_SetPeltierControl(System.IntPtr DeviceHandle, ushort ControlFlag);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_GetPeltierStatus(System.IntPtr DeviceHandle);

        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_GetCounterData(System.IntPtr DeviceHandle, System.IntPtr PipeHandle, ushort Size, uint[] DataArea);
        [DllImport("mppcum1a.dll")]
        public extern static ushort MPPC_GetCounterDataCooled(System.IntPtr DeviceHandle, System.IntPtr PipeHandle, ushort Size, uint[] DataArea);


        [DllImport("mppcum1a.dll")]
        public extern static void MPPC_GetDllVersion(System.Text.StringBuilder Version);
        [DllImport("mppcum1a.dll")]
        public extern static void MPPC_GetSysVersion(System.Text.StringBuilder Version);
    
    }
}
