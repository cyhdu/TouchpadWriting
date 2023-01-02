using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private List<int> GetInput(IntPtr lParam, ref List<TouchContact> curList)
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
            }
            finally
            {
                Marshal.FreeHGlobal(rawInputPointer);
            }
            /* GET RAW HID REPORT */

            IntPtr rawHIDDataPointer = Marshal.AllocHGlobal(rawHIDdata.Length);
            Marshal.Copy(rawHIDdata, 0, rawHIDDataPointer, rawHIDdata.Length);
            
            IntPtr preparsedDataPointer = IntPtr.Zero;
            List<int> updateList = new List<int>();
            try
            {
                uint preparsedDataSize = 0;
                if (GetRawInputDeviceInfo(rawinput.Header.hDevice, RIDI_PREPARSEDDATA, IntPtr.Zero, ref preparsedDataSize) != 0) { return null; }

                preparsedDataPointer = Marshal.AllocHGlobal((int)preparsedDataSize);
                if (GetRawInputDeviceInfo(rawinput.Header.hDevice, RIDI_PREPARSEDDATA, preparsedDataPointer, ref preparsedDataSize) < 0) { return null; }

                if (HidP_GetCaps(preparsedDataPointer, out HIDP_CAPS caps) != HIDP_STATUS_SUCCESS) { return null; }

                ushort numValCaps = caps.NumberInputValueCaps;
                var valCaps = new HIDP_VALUE_CAPS[numValCaps];

                if (HidP_GetValueCaps(HIDP_REPORT_TYPE.HidP_Input, valCaps, ref numValCaps, preparsedDataPointer) != HIDP_STATUS_SUCCESS) { return null; }
                

                uint scanTime = 0;
                uint contactCount = 0;
                
                foreach (var v in valCaps.OrderBy(x => x.LinkCollection))
                {
                    if (HidP_GetUsageValue(HIDP_REPORT_TYPE.HidP_Input, v.UsagePage, v.LinkCollection, v.UsageMin,
                            out uint value, preparsedDataPointer, rawHIDDataPointer, (uint)rawHIDdata.Length) !=
                        HIDP_STATUS_SUCCESS)
                    {
                        continue;
                    }
                    if (v.LinkCollection == 0)
                    {
                        if (v.UsagePage == 0x0D && v.Usage == 0x54)
                        {
                            contactCount = value;
                            break;
                        }
                    }
                }

                for (int contact = 0; contact < contactCount; contact++)
                {
                    TouchContact t = new();
                    t.ContactID = (uint?)(rawHIDdata[(1 + 5*contact)%rawHIDdata.Length] & 0xfc)>>2;
                    t.touching = (rawHIDdata[(1 + 5*contact)%rawHIDdata.Length] & 0x02)>>1;
                    // X = 1 and 2
                    // Y = 3 and 4
                    // TLC is 0, so add 1
                    t.X = (uint?)(rawHIDdata[(2 + 5*contact)%rawHIDdata.Length] + 256*rawHIDdata[(3 + 5*contact)%rawHIDdata.Length]);
                    t.Y = (uint?)(rawHIDdata[(4 + 5*contact)%rawHIDdata.Length] + 256*rawHIDdata[(5 + 5*contact)%rawHIDdata.Length]);
                    if (t != touches[(int)t.ContactID])
                    {
                        curList[(int)t.ContactID] = t;
                        updateList.Add((int)t.ContactID);
                    }
                }

                s += '\n';

            }
            finally
            {
                s += '\n';
                TextBlock1.Text = s;
                Marshal.FreeHGlobal(rawHIDDataPointer);
                Marshal.FreeHGlobal(preparsedDataPointer);
            }

            return updateList;
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
            for (int i = 0; i < 5; i++)
            {
                touches.Add(new TouchContact());
            }

            if (!success) return;
            Console.WriteLine("YAYY");
            TextBlock1.Text = "SUCCESS";
        }


        private const double sX = (1350 / 600);
        private const double sY = (690 / 300);
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_INPUT:
                    DrawingAttributes drawingAttributes1 = new DrawingAttributes();
                    drawingAttributes1.Color = Colors.Green;
                    var prevTouches = new List<TouchContact>(touches);
                    var updates = GetInput(lParam, ref touches);
                    foreach (var i in updates)
                    {
                        StylusPoint spp;
                        if (prevTouches[i].touching == 0)
                        {
                            spp = new StylusPoint((double)touches[i].X/sX, (double)touches[i].Y/sY);
                        }
                        else
                        {
                            spp = new StylusPoint((double)prevTouches[i].X/sX, (double)prevTouches[i].Y/sY);
                        }
                        // updateString();
                        StylusPoint sp1 = new StylusPoint((double)touches[i].X/sX, (double)touches[i].Y/sY);
                        StylusPointCollection points = new StylusPointCollection(
                            new StylusPoint[] {spp, sp1});
                        IC1.Strokes.Add(new Stroke(points, drawingAttributes1));
                    }
                    
                    break;
            }

            return IntPtr.Zero;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Process(e);
        }

        private void Process(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Y:
                    IC1.Strokes.Clear();
                    break;
            }
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