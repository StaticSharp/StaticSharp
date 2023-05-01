using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StaticSharpDemo.Utils {

    public static class TextFormatting {
        public static Paragraph ToLandingSectionHeader(this string text, Color highlightColor, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            var result = new Paragraph(callerLineNumber, callerFilePath);
            var textFragments = new List<string>();
            var startIndex = 0;
            bool upperCase = true;
            while (startIndex < text.Length) {
                var fragment = string.Concat(text.Skip(startIndex).TakeWhile(x => char.ToLower(x) == char.ToUpper(x) || (x == char.ToLower(x)) ^ upperCase));
                if (fragment.Length > 0) {
                    startIndex += fragment.Length;
                    if (upperCase) {
                        result.Inlines.Add(new Inline(fragment) { ForegroundColor = highlightColor });
                    } else {
                        result.Inlines.Add(fragment.ToUpper());
                    }
                }
                upperCase = !upperCase;
            }
            return result.ToLandingSectionHeader();
        }

        public static Paragraph ToLandingSectionHeader(this Paragraph x) {
            x.FontFamilies = new() { "Inter" };
            x.FontSize = 50;
            x.Weight = FontWeight.ExtraLight;
            x.LineHeight = 1.2;
            x.MarginTop = 60;
            return x;
        }

        public static Paragraph ToLandingMainHeader(this Paragraph x) {
            x = x.ToLandingSectionHeader();
            x.FontSize = 90;
            return x;
        }

        public static Paragraph ToSectionHeader(this string text, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            var id = text.Replace(" ", "_");
            return new Paragraph(text, callerLineNumber, callerFilePath) {
                MarginTop = 40,
                FontSize = 50,
                MarginLeft = 60,
                Weight = FontWeight.ExtraLight,

                Modifiers = { 
                    new Id{ 
                        Value = id
                    }
                },

                UnmanagedChildren = { 
                    new Block{ 
                        X = new(e=>-e.Width),
                        Width = new(e=>e.Parent.MarginLeft),
                        Height = new(e=>e.Parent.Height),
                        ExternalLink = $"#{id}",

                        Modifiers = { 
                            new Hover()
                        },

                        UnmanagedChildren = {
                            new SvgIconBlock(SvgIcons.MaterialDesignIcons.LinkVariant){
                                Visibility = new (e=>e.Parent.AsHover().Value ? 0.25 : 0.1),
                                Width= 35,
                            }.Center()
                        }                        
                    },                    
                }
            };
        }

    }

}