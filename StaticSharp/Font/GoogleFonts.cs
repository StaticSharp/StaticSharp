using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace StaticSharp.Gears {
    public static class GoogleFonts {

        public class FontInfo {
            public string FamilyName = null!;
            public bool Italic;
            public int Weight;
            public Uri Url = null!;
            public List<Segment> Segments = new();
            public string GetFileName() {
                return Path.GetFileName(Url.AbsolutePath);
            }
        }

        public static string UrlPrefix = "https://fonts.googleapis.com/css2";

        static string FormatFontFamily(string x) {
            return x.Replace(' ', '+');
        }

        public static string MakeCssUrl(string fontFamily) {

            var weightParameter = String.Join(';', Enumerable.Range(0, 2).SelectMany(i =>
                 Enumerable.Range(1, 10).Select(w => $"{i},{w}00")
                ));
            return $"https://fonts.googleapis.com/css2?family={FormatFontFamily(fontFamily)}:ital,wght@{weightParameter}";
        }

        public static string MakeCssUrl(string fontFamily, FontWeight weight, bool italic, string text = "") {

  
            var result = $"{UrlPrefix}?family={FormatFontFamily(fontFamily)}:ital,wght@{(italic ? 1 : 0)},{(int)weight}";
            if (!string.IsNullOrEmpty(text)) {
                var escapedText = Uri.EscapeDataString(text);

                result = result + "&text=" + escapedText;
            }
            return result;
        }


        static Regex UnicodeRangeItemRegex = new Regex(@"U\+([0-9a-fA-F]+)(?>-([0-9a-fA-F]+))?");
        static List<Segment> ParseRanges(string ranges) {
            return UnicodeRangeItemRegex.Matches(ranges).Select(
                x => {
                    var offset = int.Parse(x.Groups[1].Value, System.Globalization.NumberStyles.HexNumber);
                    var end = ((x.Groups[2].Value.Length > 0) ? int.Parse(x.Groups[2].Value, System.Globalization.NumberStyles.HexNumber) : offset);
                    return new Segment(offset, end-offset +1);
                }
                ).ToList();
        }

        static Regex FontFamilyRegex = new Regex(@"font-family\s*:\s*'([^']+)'");
        static Regex FontStyleRegex = new Regex(@"font-style\s*:\s*(.*);");
        static Regex FontWeightRegex = new Regex(@"font-weight\s*:\s*(.*);");

        static Regex UrlRegex = new Regex(@"src\s*:\s*url\s*\(([^\)]*)\)"); //parse only src->url (without format)
        static Regex UnicodeRangeRegex = new Regex(@"unicode-range\s*:\s*(.*);");



        public static FontInfo ParseCssFontFace(string cssFontFace) {
            var name = FontFamilyRegex.Match(cssFontFace).Groups[1].Value;
            var style = FontStyleRegex.Match(cssFontFace).Groups[1].Value;
            var weight = FontWeightRegex.Match(cssFontFace).Groups[1].Value;
            var url = UrlRegex.Match(cssFontFace).Groups[1].Value;
            var unicodeRange = UnicodeRangeRegex.Match(cssFontFace).Groups[1].Value;

            return new FontInfo() {
                FamilyName = name,
                Italic = style.ToLower() == "italic",
                Weight = int.Parse(weight),
                Url = new Uri(url),
                Segments = ParseRanges(unicodeRange)
            };

        }


        static Regex CssRegex = new Regex(@"@font-face\s*{([^}]*)}");
        public static IEnumerable<FontInfo> ParseCss(string css) {
            var matches = CssRegex.Matches(css);

            return matches.Select(x => ParseCssFontFace(x.Groups[1].Value));

        }


        public static HttpRequestMessage MakeWoff2Request(string url) {
            return new HttpRequestMessage(HttpMethod.Get, url) {
                Headers = {
                    {"User-Agent", "Mozilla/5.0 AppleWebKit/537 Chrome/52"}
                }
            };
        }


    }
}
