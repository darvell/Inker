using System.Runtime.InteropServices;
using System.Windows.Media;

namespace Inker.Utilities
{
    public static class Colorization
    {
        [DllImport("dwmapi.dll", EntryPoint = "#127", PreserveSig = false)]
        private static extern void DwmGetColorizationParameters(out DWMCOLORIZATIONPARAMS parameters);

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