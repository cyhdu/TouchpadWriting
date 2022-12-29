using System.Runtime.InteropServices;

namespace HIDAccess.Win32Structs;

[StructLayout(LayoutKind.Sequential)]
public struct RID_DEVICE_INFO
{
    public uint                cbSize;
    public uint                dwtype;
    public RID_DEVICE_INFO_HID hid;
}