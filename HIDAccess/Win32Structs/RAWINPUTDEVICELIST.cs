using System.Runtime.InteropServices;

namespace HIDAccess.Win32Structs;

[StructLayout(LayoutKind.Sequential)]
public struct RAWINPUTDEVICELIST
{
        public IntPtr hDevice;
        public uint dwType;
}
