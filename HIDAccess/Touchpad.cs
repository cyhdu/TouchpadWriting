using System.Runtime.InteropServices;
using HIDAccess.Win32Structs;
using static HIDAccess.RawInputDevices;

namespace HIDAccess;

public class Touchpad
{
        public static bool RegisterInput(IntPtr windowHandle)
        {

            var device = new RAWINPUTDEVICE
            {
                usUsagePage = 0x000D,
                usUsage = 0x0005,
                dwFlags = 0,
                hwndTarget = windowHandle

            };

            return RegisterRawInputDevices(new[] { device }, 1, (uint)Marshal.SizeOf<RAWINPUTDEVICE>());
        }
    
}