using System.Runtime.InteropServices;
using HIDAccess.Win32Structs;

namespace HIDAccess;

public static class RawInputDevices
{
    #region Constants
    
		public const uint RIM_TYPEMOUSE = 0;
		public const uint RIM_TYPEKEYBOARD = 1;
		public const uint RIM_TYPEHID = 2;

		public const int WM_INPUT = 0x00FF;
		
		public const uint RID_INPUT = 0x10000003;
		
		public const uint RIDI_PREPARSEDDATA = 0x20000005;
		public const uint RIDI_DEVICEINFO = 0x2000000b;

    #endregion
    
    [DllImport("User32", SetLastError = true)]
    public static extern uint GetRawInputDeviceList(
        [Out] RAWINPUTDEVICELIST[] pRawInputDeviceList,
        ref uint          puiNumDevices,
        uint              cbSize);
    
    [DllImport("User32", SetLastError = true)]
    public static extern bool RegisterRawInputDevices(
		RAWINPUTDEVICE[] pRawInputDevices,
		uint             uiNumDevices,
		uint             cbSize);
    
    [DllImport("User32", SetLastError = true)]
    public static extern uint GetRawInputData(
	    IntPtr hRawInput,
        uint      uiCommand,
        IntPtr    pData,
        ref uint    pcbSize,
        uint      cbSizeHeader);
    
    
    [DllImport("User32", SetLastError = true)]
    public static extern uint GetRawInputDeviceInfo(
		IntPtr hDevice,
		uint   uiCommand, // For RIDI_PREPARSEDDATA
		IntPtr pData,
		ref uint pcbSize);
    
    [DllImport("User32", SetLastError = true)]
    public static extern uint GetRawInputDeviceInfo(
		IntPtr hDevice,
		uint   uiCommand, // For RIDI_DEVICEINFO
		ref RID_DEVICE_INFO pData,
		ref uint pcbSize);
}