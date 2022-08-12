using StaticSharp.Symbolic;
using StaticSharpWeb;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StaticSharpDemo.Root {
    public partial class Material : StaticSharpWeb.Material {

        /*public override Footer Footer => new() {
        };

        public override RightSideBar RightSideBar => new() {
            new LanguageMenu<Language>(VirtualNode)
        };

        ;*/

        public override IBlock LeftSideBar => new Column() {

            Modifiers = {
                new Modifier() {
                    Bindings = {
                        BackgroundColor = e=>Color.FromArgb(255,0xA0,0x70,0x30),
                    }
                }
            },

            Children = {
                "Menu Item 1",
                "Menu item 2",
                new Space(){
                    Bindings = {
                        GrowBetween = e=>1
                    }
                },

                new Paragraph(){
                    ["Width"] = "() => element.LayoutWidth",
                    Children = {
                        "Social links"
                    }

                }
            }

        };


        public static Paragraph FooterTitle(string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            var paragraph = new Paragraph(callerFilePath, callerLineNumber) {
                Modifiers = {
                    new Modifier(callerFilePath, callerLineNumber) {
                        Bindings = {
                                FontSize = e=>18,
                            },
                        FontStyle = new FontStyle(FontWeight.Bold)

                    }
                }
            };
            paragraph.AppendLiteral(text, callerFilePath, callerLineNumber);
            return paragraph;
        }

        public override IBlock? Footer => new Row {
            Modifiers = {
                new Modifier(){ 
                    Bindings = { 
                        FontSize = e=>16,
                        BackgroundColor = e=>Color.Black,
                    }
                }
            },
            Bindings = {
                X = e=>0,
                Width = e=>e.ParentBlock.Width,

                PaddingTop = e=>20,
                PaddingBottom = e=>20,

                PaddingLeft = e => e.ParentBlock.PaddingLeft,
                PaddingRight = e => e.ParentBlock.PaddingRight,
            },

            Children = {

                new Blocks(){
                    new Column{
                        FooterTitle("Links"),
                        "тут будут ссылки"
                    },
                    Space.Grow(),
                    new Column{
                        FooterTitle("Column 2"),
                        "тут будут еще ссылки"

                    },
                    Space.Grow(),
                    new Column{
                        FooterTitle("Column 3"),
                        "и тут будут ссылки",
                        "line 2",
                        "line 3"
                    }

                }.Modify(x=>{
                    foreach (var c in x.Values.OfType<Column>()){

                        c.Bindings.MarginLeft = e=> 10;
                        c.Bindings.MarginRight = e=> 10;

                        foreach (var b in c.Children.Values.OfType<Block>()){
                            b.Bindings.MarginLeft = e=>0;
                            b.Bindings.MarginRight = e=>0;
                        }
                    }
                })

            }
        };




    }

}
