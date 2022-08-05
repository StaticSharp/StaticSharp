using StaticSharp.Symbolic;
using StaticSharpWeb;

using System.Collections.Generic;
using System.Drawing;

namespace StaticSharpDemo.Root {
    partial class Material : StaticSharpWeb.Material {

        /*public override Footer Footer => new() {
        };

        public override RightSideBar RightSideBar => new() {
            new LanguageMenu<Language>(VirtualNode)
        };

        ;*/

        public override IBlock LeftSideBar => new Column() {

            Modifiers = {
                new Modifier() {
                    BackgroundColor = Color.FromArgb(255,0xA0,0x70,0x30),
                }
            },

            Children = {
                "Menu Item 1",
                "Menu item 2",
                new Space(){
                    GrowBetween = 1
                },

                new Paragraph(){
                    Width = "() => element.LayoutWidth",
                    Children = {
                        "Social links"
                    }

                }
            }

        };

        public override IBlock? Footer => new Row{
            MarginLeft = 8,
            PaddingLeft = -8,
            Children = { 
                new Column{
                    H1("Links"),
                    "тут будут ссылки"
                },
                new Column{
                    H1("Column 2"),
                    "тут будут еще ссылки"
                },
                new Column{
                    H1("Column 3"),
                    "и тут будут ссылки",
                    "line 2",
                    "line 3"
                }
            }
        };




    }

}
