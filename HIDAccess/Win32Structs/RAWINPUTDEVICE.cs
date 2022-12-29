using System.Runtime.InteropServices;

namespace HIDAccess.Win32Structs;

[StructLayout(LayoutKind.Sequential)]
public struct RAWINPUTDEVICE
{
    public ushort usUsagePage;
    public ushort usUsage;
    public uint  dwFlags;
    public IntPtr   hwndTarget;
    
}
