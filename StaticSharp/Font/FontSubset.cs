using StaticSharp.Gears;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {


    public enum DefaultFont {
        Arial,
        TimesNewRoman,
        CourierNew,
    }

    namespace Gears {
        public static partial class FontUtils {
            
            static UnicodeCategory[] NonRenderingCategories = new UnicodeCategory[] {
            UnicodeCategory.Control,
            UnicodeCategory.OtherNotAssigned,
            UnicodeCategory.Surrogate };
            static bool IsPrintable(char c) {
                if (char.IsWhiteSpace(c))
                    return false;
                if (NonRenderingCategories.Contains(char.GetUnicodeCategory(c)))
                    return false;
                return true;
            }

            public static HashSet<char> ToPrintableChars(this string text) {
                var result = new HashSet<char>();
                foreach (var c in text) {
                    if (IsPrintable(c))
                        result.Add(c);
                }
                return result;
            }
        }
    }



    public class FontSubset {

        private Font font;
        private HashSet<char> usedChars = new();
        public FontSubset(Font font) {
            this.font = font;
        }

        public HashSet<char> AddChars(HashSet<char> chars) {
            var existingChars = font.GetExistingChars(chars);
            usedChars.UnionWith(existingChars);
            chars = new HashSet<char>(chars);
            chars.ExceptWith(existingChars);
            return chars;
        }

        public string GenerateInclude() {

            var sortedUsedChars = usedChars.OrderBy(c => c);
            var text = string.Concat(sortedUsedChars);

            var subfontCssUrl = GoogleFonts.MakeCssUrl(font.Family.Name, font.Weight, font.Italic, text);
            var subFontCssRequest = new HttpRequestGenome(GoogleFonts.MakeWoff2Request(subfontCssUrl)).Result;

            var fontInfos = GoogleFonts.ParseCss(subFontCssRequest.Text);
            //TODO validation
            var fontInfo = fontInfos.First();
            var subFontRequest = new HttpRequestGenome(fontInfo.Url).Result;

            var content = subFontRequest.Data;

            var base64 = Convert.ToBase64String(content);
            var format = "woff2";

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("@font-face {");
            stringBuilder.Append($"font-family: '{font.Family.Name}';");
            stringBuilder.Append("font-weight: ").Append((int)font.Weight).Append(";");
            stringBuilder.Append("font-style: ").Append(Font.ItalicToStyle(font.Italic)).Append(";");
            //stringBuilder.AppendLine($"src:local('{Arguments.Family} {Arguments.Weight}{italicSuffix}'),");
            stringBuilder.Append($"src: url(data:application/font-{format};charset=utf-8;base64,{base64}) format('{format}');");
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}

