using System;
using System.Drawing;
using System.IO;
using System.Threading;

namespace StaticSharpWeb {
    public static class ColorStatic {
        public static Color Lerp(this Color s, Color t, float k) {
            var bk = (1 - k);
            var a = s.A * bk + t.A * k;
            var r = s.R * bk + t.R * k;
            var g = s.G * bk + t.G * k;
            var b = s.B * bk + t.B * k;
            return Color.FromArgb((int)a, (int)r, (int)g, (int)b);
        }

        public static uint ToRgba(this Color color) {
            var argb = (uint)color.ToArgb();
            uint rgba = argb << 8;
            rgba |= argb >> 24;
            return rgba;
        }

        public static string ToRgbaHexString(this Color color) {
            return color.ToRgba().ToString("X8");
        }

        public static uint ToRgb(this Color color) {
            var argb = (uint)color.ToArgb();
            uint rgba = argb & 0xFFFFFF;
            return rgba;
        }
        public static string ToRgbHexString(this Color color) {
            return color.ToRgb().ToString("X6");
        }

    }
}