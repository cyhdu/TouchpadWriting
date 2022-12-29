using System.Runtime.InteropServices;
using HIDAccess.HIDStructs;

namespace HIDAccess;

public static class HID
{
    #region Constants
    
		public const uint HIDP_STATUS_SUCCESS = 0x00110000;
    
    #endregion
    
    [DllImport("HID", SetLastError = true)]
    public static extern uint HidP_GetCaps(
        IntPtr              PreparsedData,
        out HIDP_CAPS       Capabilities);
    
    [DllImport("HID", CharSet = CharSet.Auto)]
    public static extern uint HidP_GetValueCaps(
        HIDP_REPORT_TYPE        ReportType, 
        [Out] HIDP_VALUE_CAPS[] ValueCaps,
        ref ushort              ValueCapsLength, 
        IntPtr                  PreparsedData);
    
    [DllImport("HID", CharSet = CharSet.Auto)]
    public static extern uint HidP_GetUsageValue(
        HIDP_REPORT_TYPE     ReportType, 
        ushort               UsagePage,
        ushort               LinkCollection,
        ushort               Usage,
        out uint             UsageValue,
        IntPtr               PreparsedData,
        IntPtr               Report,
        uint                 ReportLength); 
}