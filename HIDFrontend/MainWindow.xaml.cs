using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using HIDAccess;
using HIDAccess.HIDStructs;
using HIDAccess.Win32Structs;
using static HIDAccess.RawInputDevices;
using static HIDAccess.HID;

namespace HIDFrontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<TouchContact> touches = new List<TouchContact>();
        private string sss = "";
        private uint currentID = 12;
        
        private static bool RegisterInput(IntPtr windowHandle)
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

        private TouchContact GetInput(IntPtr lParam)
        {
            /* GET RAW HID REPORT */
            uint dwSize = 0;
            uint rawInputHeaderSize = (uint)Marshal.SizeOf<RAWINPUTHEADER>();
            if (GetRawInputData(lParam, RID_INPUT, IntPtr.Zero, ref dwSize, rawInputHeaderSize) != 0) { return null; }

            RAWINPUT rawinput;
            byte[] rawHIDdata;

            IntPtr rawInputPointer = IntPtr.Zero;
            rawInputPointer = Marshal.AllocHGlobal((int)dwSize);
            string s = "";

            try
            {
                if (GetRawInputData(lParam, RID_INPUT, rawInputPointer, ref dwSize, rawInputHeaderSize) != dwSize)
                {
                    return null;
                }

                rawinput = Marshal.PtrToStructure<RAWINPUT>(rawInputPointer);

                var rawInputData = new byte[dwSize];
                Marshal.Copy(rawInputPointer, rawInputData, 0, rawInputData.Length);

                rawHIDdata = new byte[rawinput.Hid.dwSizeHid * rawinput.Hid.dwCount];
                int rawInputOffset = (int)dwSize - rawHIDdata.Length;
                Buffer.BlockCopy(rawInputData, rawInputOffset, rawHIDdata, 0, rawHIDdata.Length);

                // foreach (var v in rawHIDdata)
                // {
                //     s += $"{Convert.ToString(v, 2).PadLeft(8, '0')} ";
                // }
            }
            finally
            {
                Marshal.FreeHGlobal(rawInputPointer);
            }
            /* GET RAW HID REPORT */

            IntPtr rawHIDDataPointer = Marshal.AllocHGlobal(rawHIDdata.Length);
            Marshal.Copy(rawHIDdata, 0, rawHIDDataPointer, rawHIDdata.Length);
            
            IntPtr preparsedDataPointer = IntPtr.Zero;
            TouchContact t = new();
            try
            {
                uint preparsedDataSize = 0;
                if (GetRawInputDeviceInfo(rawinput.Header.hDevice, RIDI_PREPARSEDDATA, IntPtr.Zero, ref preparsedDataSize) != 0) { return null; }

                preparsedDataPointer = Marshal.AllocHGlobal((int)preparsedDataSize);
                if (GetRawInputDeviceInfo(rawinput.Header.hDevice, RIDI_PREPARSEDDATA, preparsedDataPointer, ref preparsedDataSize) < 0) { return null; }

                if (HidP_GetCaps(preparsedDataPointer, out HIDP_CAPS caps) != HIDP_STATUS_SUCCESS) { return null; }

                ushort numValCaps = caps.NumberInputValueCaps;
                ushort numCaps = caps.NumberInputButtonCaps;
                var valCaps = new HIDP_VALUE_CAPS[numValCaps];
                var butCaps = new HIDP_BUTTON_CAPS[numCaps];


                if (HidP_GetValueCaps(HIDP_REPORT_TYPE.HidP_Input, valCaps, ref numValCaps, preparsedDataPointer) != HIDP_STATUS_SUCCESS) { return null; }
                // if (HidP_GetButtonCaps(HIDP_REPORT_TYPE.HidP_Input, butCaps, ref numCaps, preparsedDataPointer) != HIDP_STATUS_SUCCESS) { return null; }
                //
                // if (HidP_GetUsages(HIDP_REPORT_TYPE.HidP_Input, butCaps[0].UsagePage, 0, 
                //         gb, g, preparsedDataPointer,
                //         rawHIDDataPointer, (uint)rawHIDdata.Length) !=
                //     HIDP_STATUS_SUCCESS)
                // {
                //     return null;
                // }
                
                // foreach (var v in (butCaps))
                // {
                //     s += $"{v.UsagePage}";
                // }
                
                

                // s += "\n\t";
                uint scanTime = 0;
                uint contactCount = 0;
                // s += "\n HERE \n";
                // s += valCaps.Length + "\n";
                var touching = (rawHIDdata[1] & 0x02)>>1;
                s += $"\n{touching} \n";
                t.touching = touching;
                
                foreach (var v in valCaps.OrderBy(x => x.LinkCollection))
                {
                    s += $"{v.LinkCollection} {v.UsagePage:X2} {v.Usage:X2}";
                    if (HidP_GetUsageValue(HIDP_REPORT_TYPE.HidP_Input, v.UsagePage, v.LinkCollection, v.UsageMin,
                            out uint value, preparsedDataPointer, rawHIDDataPointer, (uint)rawHIDdata.Length) !=
                        HIDP_STATUS_SUCCESS)
                    {
                        // s += "  ERROR\n";
                        continue;
                    }

                    if (v.LinkCollection == 0)
                    {
                        continue;
                    }
       //              switch (v.LinkCollection)
       //              {
       //                  case 0:
							// switch (v.UsagePage, v.Usage)
							// {
							// 	case (0x0D, 0x56): // Scan Time
							// 		scanTime = value;
							// 		break;
       //
							// 	case (0x0D, 0x54): // Contact Count
							// 		contactCount = value;
							// 		break;
							// }
       //
       //                      s += $"     {contactCount}\n";
       //                      break;
       //                  
       //                  default:
                            switch (v.UsagePage, v.Usage)
                            {
                                case (0x0D, 0x51):
                                    // s += $" CONTACT ID {value:D}";
                                    t.ContactID = value;
                                    break;
                                case (0x01, 0x30):
                                    // s += $" X POSITION {value:D}";
                                    t.X = value;
                                    break;
                                case (0x01, 0x31):
                                    // s += $" Y POSITION {value:D}";
                                    t.Y = value;
                                    break;
                            }
                    }
                // }

            }
            finally
            {
                TextBlock1.Text = s;
                Marshal.FreeHGlobal(rawHIDDataPointer);
                Marshal.FreeHGlobal(preparsedDataPointer);
            }

            if (t.IsValid())
            {
                return t;
            }
            return new TouchContact();
        }


        public MainWindow()
        {
            InitializeComponent();

        }

        private HwndSource _targetSource;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _targetSource = PresentationSource.FromVisual(this) as HwndSource;
			_targetSource?.AddHook(WndProc);
            
            var success = RegisterInput(_targetSource.Handle);
            for (int i = 0; i < 8; i++)
            {
                touches.Add(new TouchContact());
            }

            if (!success) return;
            Console.WriteLine("YAYY");
            TextBlock1.Text = "SUCCESS";
        }


        private const double sX = (3500 / 600);
        private const double sY = (2000 / 300);
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_INPUT:
                    DrawingAttributes drawingAttributes1 = new DrawingAttributes();
                    drawingAttributes1.Color = Colors.Green;
                    var t = GetInput(lParam);
                    var prev = touches[(int)t.ContactID];
                    StylusPoint spp;
                    if (prev.touching == 0)
                    {
                        spp = new StylusPoint((double)t.X/sX, (double)t.Y/sY);
                    }
                    else
                    {
                        spp = new StylusPoint((double)prev.X/sX, (double)prev.Y/sY);
                    }
                    touches[(int)t.ContactID] = t;
                    // updateString();
                    StylusPoint sp1 = new StylusPoint((double)t.X/sX, (double)t.Y/sY);
                    StylusPointCollection points = new StylusPointCollection(
                        new StylusPoint[] {spp, sp1});
                    IC1.Strokes.Add(new Stroke(points, drawingAttributes1));
                    break;
            }

            return IntPtr.Zero;
        }

        private void updateString()
        {
            sss = "";
            foreach (var t in touches)
            {
                sss += t;
                sss += "\n";
            }

            TextBlock2.Text = sss;

        }

    }
}