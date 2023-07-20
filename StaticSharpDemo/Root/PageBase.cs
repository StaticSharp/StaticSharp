using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        protected new Inline Italic(string text, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return new Inline(text, callerLineNumber, callerFilePath) {
                Italic = true,
            };
        }


        protected Paragraph ListItem(
            Inlines inlines, string bulletIcon = "&bull; ", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {

            return new Paragraph(inlines) {
                MarginLeft = new(e => e.UnmanagedChildren.First().Width),
                UnmanagedChildren = {
                    new Paragraph(bulletIcon) {
                        X = new(e=>-e.Width),
                        //Width = new(e=>e.Parent.MarginLeft),
                        Height = new(e=>e.Parent.Height)
                    },
                }
            };
        }

        protected Block ListItem(Inline inline, string bulletIcon = "&bull; ", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return ListItem(new Inlines() { inline }, bulletIcon);
        }

        protected Block ListItem2(Inlines inlines, string bulletIcon = "&bull;", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            var bulletWidth = 10;
            return new LinearLayout {
                Vertical = false,
                ItemGrow = 0,
                Children = {
                    new Paragraph(bulletIcon)
                    {
                        Width = bulletWidth
                    },
                    new Paragraph(inlines) {
                        Width = new (e => e.Parent.Width - bulletWidth)
                    }
                }
            };
        }

        protected Block ListItem2(Inline inline, string bulletIcon = "&bull;", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return ListItem2(new Inlines() { inline }, bulletIcon);
        }

        protected Inline FilePath(string text, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return Bold(text); // TODO: fix and use italic?
        }

        protected Inline Code(string text, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return Code(new Inlines {
                text
            }, callerLineNumber, callerFilePath);
        }

        protected Inline Code(Inlines inlines, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return new Inline(inlines, callerLineNumber, callerFilePath) {
                FontFamilies = CodeFontFamilies,
                Weight = FontWeight.Regular,
            };
        }
        protected Inline PropertyName(string name, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return Code(name, callerLineNumber, callerFilePath).Modify(x => x.ForegroundColor = Color.BlueViolet);
        }

        protected Inline TypeName<T>([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return Code(typeof(T).Name, callerLineNumber, callerFilePath).Modify(x => x.ForegroundColor = Color.OrangeRed);
        }



        public string TitleToId(string title) => title.Replace(" ", "_");

        /// <summary>
        /// Link to the same page header created by <see cref="PageBase.SectionHeader(string, int, string)">SectionHeader</see> method
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Inline component representing a hyperlink</returns>
        public Inline SectionHeaderLink(string title) {
            return new Inline(title) {
                ExternalLink = $"#{TitleToId(title)}"
            };
        }

        protected Paragraph SectionHeader(string text, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            var id = TitleToId(text);
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
