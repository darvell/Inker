using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Inker.Utilities
{
    public static class Colorization
    {
        [DllImport("dwmapi.dll", EntryPoint = "#127", PreserveSig = false)]
        private static extern void DwmGetColorizationParameters(out DWMCOLORIZATIONPARAMS parameters);

        private const int WM_DWMCOLORIZATIONCOLORCHANGED = 0x320;

        private static bool _hookedColorization = false;

        internal static EventHandler<Color> _colorDelegate;

        public static event EventHandler<Color> ColorizationChanged
        {
            add
            {
                if (!_hookedColorization)
                {
                    ConfigureMonitoring();
                }
                _colorDelegate += value;
            }
            remove => _colorDelegate -= value;
        }

        private static void ConfigureMonitoring()
        {
            IntPtr mainHandle = Process.GetCurrentProcess().MainWindowHandle;
            if (mainHandle != IntPtr.Zero)
            {
                HwndSource.FromHwnd(mainHandle)?.AddHook(WndProcHook);
                Debug.WriteLine("Hook added.");
            }
            else
            {
                throw new Exception("Attempted to listen to colorization changes prior to application load.");
            }
            _hookedColorization = true;
        }

        private static IntPtr WndProcHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_DWMCOLORIZATIONCOLORCHANGED:
                    _colorDelegate.Invoke(null, Current);
                    return IntPtr.Zero;

                default:
                    return IntPtr.Zero;
            }
        }

        public static Color Current
        {
            get
            {
                DwmGetColorizationParameters(out var colors);

                return Color.FromArgb(
                    (byte)(colors.ColorizationColor >> 24),
                    (byte)(colors.ColorizationColor >> 16),
                    (byte)(colors.ColorizationColor >> 8),
                    (byte)colors.ColorizationColor
                );
            }
        }

        internal struct DWMCOLORIZATIONPARAMS
        {
            public uint ColorizationColor,
                ColorizationAfterglow,
                ColorizationColorBalance,
                ColorizationAfterglowBalance,
                ColorizationBlurBalance,
                ColorizationGlassReflectionIntensity,
                ColorizationOpaqueBlend;
        }
    }
}