using System.Linq;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public static partial class DefaultStyle {

        private static string codeFontFamily = "Roboto Mono";
        private static double codeBackgroundIntensity = 0.05;

        public static double CodeBlockFontSize = 14;

        public static Inline Code(Inlines inlines, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return new Inline(callerLineNumber, callerFilePath) {
                PaddingsHorizontal = 0.25,
                PaddingTop = 0.1,
                PaddingBottom = 0.25,
                Radius = 3,
                BackgroundColor = new(e => Color.Lerp(e.Parent.HierarchyBackgroundColor, e.Parent.HierarchyForegroundColor, codeBackgroundIntensity)),
                Weight = FontWeight.Regular,
                FontFamilies = { codeFontFamily },
                Children = {
                    inlines
                }
            };
        }

        public static ScrollLayout CodeBlock(Inlines inlines, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return new ScrollLayout(callerLineNumber, callerFilePath) {
                Height = new(e => Js.Math.Min(e.InternalHeight, e.Root.Height * 0.8)),
                Radius = 8,
                BackgroundColor = new(e => Color.Lerp(e.Parent.HierarchyBackgroundColor, e.Parent.HierarchyForegroundColor, codeBackgroundIntensity)),
                Paddings = 12,
                Content = new Paragraph(
                        inlines, callerLineNumber, callerFilePath
                    ) {
                    FontSize = CodeBlockFontSize,
                    Weight = FontWeight.Regular,
                    FontFamilies = { codeFontFamily }
                },
            };
        }
        
 
        public static Inline Bold(Inlines inlines, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return new Inline(callerLineNumber, callerFilePath) {
                Weight = FontWeight.Bold,
                Children = {
                    inlines
                }
            };
        }



    }

}