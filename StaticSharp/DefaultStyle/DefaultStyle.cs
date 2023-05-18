using EnvDTE;
using StaticSharp.Js;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public static partial class DefaultStyle {

        private static string codeFontFamily = "Roboto Mono";
        private static double codeBackgroundIntensity => 0.02;

        public static double CodeBlockFontSize = 14;

        public static Inline Code(Inlines inlines, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return new Inline(callerLineNumber, callerFilePath) {
                PaddingsHorizontal = 0.25,
                PaddingTop = 0.1,
                PaddingBottom = 0.25,
                BackgroundColor = new(e => Color.Lerp(e.Parent.HierarchyBackgroundColor, e.Parent.HierarchyForegroundColor, codeBackgroundIntensity)),
                Weight = FontWeight.Regular,
                FontFamilies = { codeFontFamily },
                Children = {
                    inlines
                },
                Modifiers = {
                    new BorderRadius(){
                        Radius= 3,
                    },
                }
            };
        }

        public static ScrollView CodeBlockScrollable(Inlines inlines, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return new ScrollView(callerLineNumber, callerFilePath) {
                //PreferredHeight = new(e => Js.Math.Min(e.InternalHeight, e.Root.Height * 0.8)),
                Height = new(e => Js.Num.Min(e.InternalHeight, e.Root.Height * 0.8)),
                BackgroundColor = new(e => Color.Lerp(e.Parent.HierarchyBackgroundColor, e.Parent.HierarchyForegroundColor, codeBackgroundIntensity)),
                Paddings = 12,
                Child = new Paragraph(
                        inlines, callerLineNumber, callerFilePath
                    ) {
                    FontSize = new(e=>Num.PiecewiseLinearInterpolation(Js.Window.DevicePixelRatio,new(1,16),new(2,11))),
                    Weight = FontWeight.Regular,
                    FontFamilies = { codeFontFamily }
                },
                Modifiers = {
                    new BorderRadius(){
                        Radius = 8,
                    },
                }
            };
        }
        public static Paragraph CodeBlock(Inlines inlines, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            
            return new Paragraph(inlines, callerLineNumber, callerFilePath) {
                BackgroundColor = new(e => Color.Lerp(e.Parent.HierarchyBackgroundColor, e.Parent.HierarchyForegroundColor, codeBackgroundIntensity)),
                Paddings = 12,
                FontSize = CodeBlockFontSize,
                Weight = FontWeight.Regular,
                FontFamilies = { codeFontFamily },
                Modifiers = {
                    new BorderRadius(){
                        Radius = 8,
                    },
                }
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