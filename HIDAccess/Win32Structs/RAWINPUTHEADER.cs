using System.Runtime.InteropServices;

namespace HIDAccess.Win32Structs;

[StructLayout(LayoutKind.Sequential)]
public struct RAWINPUTHEADER
{
    public uint  dwType;
    public uint  dwSize;
    public IntPtr hDevice;
    public IntPtr wParam;
}