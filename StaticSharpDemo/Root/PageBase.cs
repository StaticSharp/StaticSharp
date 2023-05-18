using System.Runtime.CompilerServices;

namespace StaticSharpDemo.Root {
    public abstract partial class PageBase : StaticSharp.Page {

        protected virtual FontFamilies CodeFontFamilies => new() {
            new FontFamilyGenome("Roboto Mono")
        };
        protected Inline Bold(string text, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return new Inline(text, callerLineNumber, callerFilePath) { 
                Weight = FontWeight.Medium,
            };
        }

        protected Inline Code(string text, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return Code(new Inlines {
                text
            },callerLineNumber,callerFilePath);
        }

        protected Inline Code(Inlines inlines, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return new Inline(inlines,callerLineNumber,callerFilePath) {
                FontFamilies = CodeFontFamilies,
                Weight = FontWeight.Regular,
            };
        }
        protected Inline PropertyName(string name, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return Code(name, callerLineNumber, callerFilePath).Modify(x => x.ForegroundColor = Color.BlueViolet);
        }

        protected Inline TypeName<T>([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return Code(typeof(T).Name,callerLineNumber,callerFilePath).Modify(x => x.ForegroundColor = Color.OrangeRed);
        }

        protected Paragraph SectionHeader(string text, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
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
