using System.Runtime.InteropServices;

namespace HIDAccess.Win32Structs;

[StructLayout(LayoutKind.Sequential)]
public struct RID_DEVICE_INFO_HID
{
    public uint   dwVendorId;
    public uint   dwProductId;
    public uint   dwVersionNumber;
    public ushort usUsagePage;
    public ushort usUsage;
}