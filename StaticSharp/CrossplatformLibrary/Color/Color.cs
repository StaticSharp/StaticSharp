using EnvDTE80;
using ImageMagick;
using Javascriptifier;
using StaticSharp.Gears;
using System;
using System.Diagnostics.Contracts;

namespace StaticSharp {


    namespace Scripts {
        public class ColorAttribute : ScriptReferenceAttribute {
            public ColorAttribute() : base(GetScriptFilePath()) { }
        }
    }



    public partial struct Color : Javascriptifier.IStringifiable, IKeyProvider {


        public string Key => KeyUtils.Combine<Color>(ToJavascriptString());

        public string ToJavascriptString() {

            if (double.IsNaN(A))
                return $"new Color({R:0.###},{G:0.###},{B:0.###})";
            else
                return $"new Color({R:0.###},{G:0.###},{B:0.###},{A:0.###})";
        }
    }

    public partial struct Color {

        public double R { get; init; } = 0;
        public double G { get; init; } = 0;
        public double B { get; init; } = 0;

        public double A { get; init; } = double.NaN;

        public Color(string value) : this() {
            double hexToDouble255(string c) {
                return Convert.ToInt32(c, 16) / 255d;
            }
            double hexToDouble15(string c) {
                return Convert.ToInt32(c, 16) / 15d;
            }

            if (value.StartsWith('#'))
                value = value.Substring(1);

            if (value.Length == 8) {
                A = hexToDouble255(value[0..2]);
                value = value.Substring(2);
            }

            if (value.Length == 6) {
                R = hexToDouble255(value[0..2]);
                G = hexToDouble255(value[2..4]);
                B = hexToDouble255(value[4..6]);
                return;
            }

            if (value.Length == 4) {
                A = hexToDouble15(value[0..1]);
                value = value.Substring(1);
            }

            if (value.Length == 3) {
                R = hexToDouble15(value[0..1]);
                G = hexToDouble15(value[1..2]);
                B = hexToDouble15(value[2..3]);
            }
        }

        public Color(double r, double g, double b, double a = double.NaN) {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(uint argb) 
        {
            A = ((argb >> 24) & 0xff) / 255d;
            R = ((argb >> 16) & 0xff) / 255d;
            G = ((argb >> 8) & 0xff) / 255d;
            B = (argb & 0xff) / 255d;
        }


        private static double firstNotNaN(params double[] values) {
            foreach (var i in values)
                if (!double.IsNaN(i))
                    return i;
            return double.NaN;
        }

        [JavascriptMethodFormat("CplusC({0},{1})")]
        public static Color operator +(Color a, Color b) {
            return new Color(a.R + b.R, a.G + b.G, a.B + b.B, firstNotNaN(a.A + b.A, a.A, b.A));
        }

        [JavascriptMethodFormat("CplusN({0},{1})")]
        public static Color operator +(Color a, double b) {
            return new Color(a.R + b, a.G + b, a.B + b, a.A + b);
        }

        [JavascriptMethodFormat("CplusN({1},{0})")]
        public static Color operator +(double a, Color b) {
            return new Color(a + b.R, a + b.G, a + b.B, a + b.A);
        }


        [JavascriptMethodFormat("CminusC({0},{1})")]
        public static Color operator -(Color a, Color b) {
            return new Color(a.R - b.R, a.G - b.G, a.B - b.B, firstNotNaN(a.A - b.A, a.A, b.A));
        }

        [JavascriptMethodFormat("CminusN({0},{1})")]
        public static Color operator -(Color a, double b) {
            return new Color(a.R - b, a.G - b, a.B - b, a.A - b);
        }

        [JavascriptMethodFormat("NminusC({0},{1})")]
        public static Color operator -(double a, Color b) {
            return new Color(a - b.R, a - b.G, a - b.B, a - b.A);
        }



        [JavascriptMethodFormat("CmulC({0},{1})")]
        public static Color operator *(Color a, Color b) {
            return new Color(a.R * b.R, a.G * b.G, a.B * b.B, firstNotNaN(a.A * b.A, a.A, b.A));
        }

        [JavascriptMethodFormat("CmulN({0},{1})")]
        public static Color operator *(Color a, double b) {
            return new Color(a.R * b, a.G * b, a.B * b, a.A * b);
        }

        [JavascriptMethodFormat("CmulN({1},{0})")]
        public static Color operator *(double a, Color b) {
            return new Color(a * b.R, a * b.G, a * b.B, a * b.A);
        }


        /*[ConvertToJs("Color.CdivC({0},{1})")]
        public static Color operator /(Color a, Color b) {
            return new Color(a.R / b.R, a.G / b.G, a.B / b.B, firstNotNaN(a.A / b.A, a.A, b.A));
        }

        [ConvertToJs("Color.CdivN({0},{1})")]
        public static Color operator /(Color a, double b) {
            return new Color(a.R / b, a.G / b, a.B / b, a.A / b);
        }

        [ConvertToJs("Color.NdivC({1},{0})")]
        public static Color operator /(double a, Color b) {
            return new Color(a / b.R, a / b.G, a / b.B, a / b.A);
        }*/




        public static Color FromGrayscale(double value) {
            return new Color(value, value, value);
        }


        public static Color FromIntRGB(uint rgb) {
            return new Color(
                ((rgb >> 16) & 0xff) / 255d,
                ((rgb >> 8) & 0xff) / 255d,
                (rgb & 0xff) / 255d,
                double.NaN
            );
        }

        public static Color FromIntChannelsRGB(uint r, uint g, uint b)
        {
            return new Color(
                r / 255d,
                g / 255d,
                b / 255d
            );
        }
        public static Color FromIntChannelsRGBA(uint r, uint g, uint b, uint a)
        {
            return new Color(
                r / 255d,
                g / 255d,
                b / 255d,
                a / 255d
            );
        }


        public static Color Lerp(Color a, Color b, double t) {
            return a + (b - a) * t;
        }

        public Color Lerp(Color targetColor, double amount)
        {
            return Color.Lerp(this, targetColor, amount);
        }

        public Color ContrastColor(double contrast = 1)
        {
            var grayscale = (0.2125 * this.R) + (0.7154 * this.G) + (0.0721 * this.B);
            var blackOrWhite = (grayscale > 0.5) ? new Color(0, 0, 0, 1) : new Color(1, 1, 1, 1);
            return this.Lerp(blackOrWhite, contrast);
        }

        public Color ContrastColor()
        {
            return ContrastColor(1);
        }

    }


    public partial struct Color {

        public static Color Transparent => new Color(0x00FFFFFF);
        public static Color AliceBlue => FromIntRGB(0xF0F8FF);
        public static Color AntiqueWhite => FromIntRGB(0xFAEBD7);
        public static Color Aqua => FromIntRGB(0x00FFFF);
        public static Color Aquamarine => FromIntRGB(0x7FFFD4);
        public static Color Azure => FromIntRGB(0xF0FFFF);
        public static Color Beige => FromIntRGB(0xF5F5DC);
        public static Color Bisque => FromIntRGB(0xFFE4C4);
        public static Color Black => FromIntRGB(0x000000);
        public static Color BlanchedAlmond => FromIntRGB(0xFFEBCD);
        public static Color Blue => FromIntRGB(0x0000FF);
        public static Color BlueViolet => FromIntRGB(0x8A2BE2);
        public static Color Brown => FromIntRGB(0xA52A2A);
        public static Color BurlyWood => FromIntRGB(0xDEB887);
        public static Color CadetBlue => FromIntRGB(0x5F9EA0);
        public static Color Chartreuse => FromIntRGB(0x7FFF00);
        public static Color Chocolate => FromIntRGB(0xD2691E);
        public static Color Coral => FromIntRGB(0xFF7F50);
        public static Color CornflowerBlue => FromIntRGB(0x6495ED);
        public static Color Cornsilk => FromIntRGB(0xFFF8DC);
        public static Color Crimson => FromIntRGB(0xDC143C);
        public static Color Cyan => FromIntRGB(0x00FFFF);
        public static Color DarkBlue => FromIntRGB(0x00008B);
        public static Color DarkCyan => FromIntRGB(0x008B8B);
        public static Color DarkGoldenrod => FromIntRGB(0xB8860B);
        public static Color DarkGray => FromIntRGB(0xA9A9A9);
        public static Color DarkGreen => FromIntRGB(0x006400);
        public static Color DarkKhaki => FromIntRGB(0xBDB76B);
        public static Color DarkMagenta => FromIntRGB(0x8B008B);
        public static Color DarkOliveGreen => FromIntRGB(0x556B2F);
        public static Color DarkOrange => FromIntRGB(0xFF8C00);
        public static Color DarkOrchid => FromIntRGB(0x9932CC);
        public static Color DarkRed => FromIntRGB(0x8B0000);
        public static Color DarkSalmon => FromIntRGB(0xE9967A);
        public static Color DarkSeaGreen => FromIntRGB(0x8FBC8B);
        public static Color DarkSlateBlue => FromIntRGB(0x483D8B);
        public static Color DarkSlateGray => FromIntRGB(0x2F4F4F);
        public static Color DarkTurquoise => FromIntRGB(0x00CED1);
        public static Color DarkViolet => FromIntRGB(0x9400D3);
        public static Color DeepPink => FromIntRGB(0xFF1493);
        public static Color DeepSkyBlue => FromIntRGB(0x00BFFF);
        public static Color DimGray => FromIntRGB(0x696969);
        public static Color DodgerBlue => FromIntRGB(0x1E90FF);
        public static Color Firebrick => FromIntRGB(0xB22222);
        public static Color FloralWhite => FromIntRGB(0xFFFAF0);
        public static Color ForestGreen => FromIntRGB(0x228B22);
        public static Color Fuchsia => FromIntRGB(0xFF00FF);
        public static Color Gainsboro => FromIntRGB(0xDCDCDC);
        public static Color GhostWhite => FromIntRGB(0xF8F8FF);
        public static Color Gold => FromIntRGB(0xFFD700);
        public static Color Goldenrod => FromIntRGB(0xDAA520);
        public static Color Gray => FromIntRGB(0x808080);
        public static Color Green => FromIntRGB(0x008000);
        public static Color GreenYellow => FromIntRGB(0xADFF2F);
        public static Color Honeydew => FromIntRGB(0xF0FFF0);
        public static Color HotPink => FromIntRGB(0xFF69B4);
        public static Color IndianRed => FromIntRGB(0xCD5C5C);
        public static Color Indigo => FromIntRGB(0x4B0082);
        public static Color Ivory => FromIntRGB(0xFFFFF0);
        public static Color Khaki => FromIntRGB(0xF0E68C);
        public static Color Lavender => FromIntRGB(0xE6E6FA);
        public static Color LavenderBlush => FromIntRGB(0xFFF0F5);
        public static Color LawnGreen => FromIntRGB(0x7CFC00);
        public static Color LemonChiffon => FromIntRGB(0xFFFACD);
        public static Color LightBlue => FromIntRGB(0xADD8E6);
        public static Color LightCoral => FromIntRGB(0xF08080);
        public static Color LightCyan => FromIntRGB(0xE0FFFF);
        public static Color LightGoldenrodYellow => FromIntRGB(0xFAFAD2);
        public static Color LightGray => FromIntRGB(0xD3D3D3);
        public static Color LightGreen => FromIntRGB(0x90EE90);
        public static Color LightPink => FromIntRGB(0xFFB6C1);
        public static Color LightSalmon => FromIntRGB(0xFFA07A);
        public static Color LightSeaGreen => FromIntRGB(0x20B2AA);
        public static Color LightSkyBlue => FromIntRGB(0x87CEFA);
        public static Color LightSlateGray => FromIntRGB(0x778899);
        public static Color LightSteelBlue => FromIntRGB(0xB0C4DE);
        public static Color LightYellow => FromIntRGB(0xFFFFE0);
        public static Color Lime => FromIntRGB(0x00FF00);
        public static Color LimeGreen => FromIntRGB(0x32CD32);
        public static Color Linen => FromIntRGB(0xFAF0E6);
        public static Color Magenta => FromIntRGB(0xFF00FF);
        public static Color Maroon => FromIntRGB(0x800000);
        public static Color MediumAquamarine => FromIntRGB(0x66CDAA);
        public static Color MediumBlue => FromIntRGB(0x0000CD);
        public static Color MediumOrchid => FromIntRGB(0xBA55D3);
        public static Color MediumPurple => FromIntRGB(0x9370DB);
        public static Color MediumSeaGreen => FromIntRGB(0x3CB371);
        public static Color MediumSlateBlue => FromIntRGB(0x7B68EE);
        public static Color MediumSpringGreen => FromIntRGB(0x00FA9A);
        public static Color MediumTurquoise => FromIntRGB(0x48D1CC);
        public static Color MediumVioletRed => FromIntRGB(0xC71585);
        public static Color MidnightBlue => FromIntRGB(0x191970);
        public static Color MintCream => FromIntRGB(0xF5FFFA);
        public static Color MistyRose => FromIntRGB(0xFFE4E1);
        public static Color Moccasin => FromIntRGB(0xFFE4B5);
        public static Color NavajoWhite => FromIntRGB(0xFFDEAD);
        public static Color Navy => FromIntRGB(0x000080);
        public static Color OldLace => FromIntRGB(0xFDF5E6);
        public static Color Olive => FromIntRGB(0x808000);
        public static Color OliveDrab => FromIntRGB(0x6B8E23);
        public static Color Orange => FromIntRGB(0xFFA500);
        public static Color OrangeRed => FromIntRGB(0xFF4500);
        public static Color Orchid => FromIntRGB(0xDA70D6);
        public static Color PaleGoldenrod => FromIntRGB(0xEEE8AA);
        public static Color PaleGreen => FromIntRGB(0x98FB98);
        public static Color PaleTurquoise => FromIntRGB(0xAFEEEE);
        public static Color PaleVioletRed => FromIntRGB(0xDB7093);
        public static Color PapayaWhip => FromIntRGB(0xFFEFD5);
        public static Color PeachPuff => FromIntRGB(0xFFDAB9);
        public static Color Peru => FromIntRGB(0xCD853F);
        public static Color Pink => FromIntRGB(0xFFC0CB);
        public static Color Plum => FromIntRGB(0xDDA0DD);
        public static Color PowderBlue => FromIntRGB(0xB0E0E6);
        public static Color Purple => FromIntRGB(0x800080);
        public static Color RebeccaPurple => FromIntRGB(0x663399);
        public static Color Red => FromIntRGB(0xFF0000);
        public static Color RosyBrown => FromIntRGB(0xBC8F8F);
        public static Color RoyalBlue => FromIntRGB(0x4169E1);
        public static Color SaddleBrown => FromIntRGB(0x8B4513);
        public static Color Salmon => FromIntRGB(0xFA8072);
        public static Color SandyBrown => FromIntRGB(0xF4A460);
        public static Color SeaGreen => FromIntRGB(0x2E8B57);
        public static Color SeaShell => FromIntRGB(0xFFF5EE);
        public static Color Sienna => FromIntRGB(0xA0522D);
        public static Color Silver => FromIntRGB(0xC0C0C0);
        public static Color SkyBlue => FromIntRGB(0x87CEEB);
        public static Color SlateBlue => FromIntRGB(0x6A5ACD);
        public static Color SlateGray => FromIntRGB(0x708090);
        public static Color Snow => FromIntRGB(0xFFFAFA);
        public static Color SpringGreen => FromIntRGB(0x00FF7F);
        public static Color SteelBlue => FromIntRGB(0x4682B4);
        public static Color Tan => FromIntRGB(0xD2B48C);
        public static Color Teal => FromIntRGB(0x008080);
        public static Color Thistle => FromIntRGB(0xD8BFD8);
        public static Color Tomato => FromIntRGB(0xFF6347);
        public static Color Turquoise => FromIntRGB(0x40E0D0);
        public static Color Violet => FromIntRGB(0xEE82EE);
        public static Color Wheat => FromIntRGB(0xF5DEB3);
        public static Color White => FromIntRGB(0xFFFFFF);
        public static Color WhiteSmoke => FromIntRGB(0xF5F5F5);
        public static Color Yellow => FromIntRGB(0xFFFF00);
        public static Color YellowGreen => FromIntRGB(0x9ACD32);



    }
}