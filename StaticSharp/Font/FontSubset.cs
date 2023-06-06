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
        public required string Format { get; init; }
        public required string Base64 { get; init; }
        public required string FamilyName { get; init; }
        public required FontWeight Weight { get; init; }
        public required bool Italic { get; init; }
    }

    public class FontSubsetBuilder {

        private Font font;
        private HashSet<char> usedChars = new();
        public FontSubsetBuilder(Font font) {
            this.font = font;
        }

        public HashSet<char> AddChars(HashSet<char> chars) {
            var existingChars = font.GetExistingChars(chars);
            usedChars.UnionWith(existingChars);
            chars = new HashSet<char>(chars);
            chars.ExceptWith(existingChars);
            return chars;
        }


        public FontSubset GetFontSubset() {

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

            return new FontSubset {
                Format = format,
                Base64 = base64,
                FamilyName = font.Family.Name,
                Weight = font.Weight,
                Italic = font.Italic
            };
        }

        public string GenerateInclude() {
            var fontSubset = GetFontSubset();            

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("@font-face {");
            stringBuilder.Append($"font-family: '{fontSubset.FamilyName}';");
            stringBuilder.Append("font-weight: ").Append((int)fontSubset.Weight).Append(";");
            stringBuilder.Append("font-style: ").Append(Font.ItalicToStyle(fontSubset.Italic)).Append(";");
            //stringBuilder.AppendLine($"src:local('{Arguments.Family} {Arguments.Weight}{italicSuffix}'),");
            stringBuilder.Append($"src: url(data:application/font-{fontSubset.Format};charset=utf-8;base64,{fontSubset.Base64}) format('{fontSubset.Format}');");
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}

