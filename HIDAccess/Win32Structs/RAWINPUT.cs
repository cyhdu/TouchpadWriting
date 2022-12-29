using System.Runtime.InteropServices;

namespace HIDAccess.Win32Structs;

[StructLayout(LayoutKind.Sequential)]
public struct RAWINPUT
{
	public RAWINPUTHEADER Header;
	public RAWHID Hid;
}