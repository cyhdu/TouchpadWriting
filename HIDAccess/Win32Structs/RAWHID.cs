using System.Runtime.InteropServices;

namespace HIDAccess.Win32Structs;

[StructLayout(LayoutKind.Sequential)]
public struct RAWHID
{
	public uint dwSizeHid;
	public uint dwCount;
	public IntPtr bRawData;
}