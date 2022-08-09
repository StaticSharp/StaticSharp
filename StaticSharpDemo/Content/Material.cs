using StaticSharp.Symbolic;
using StaticSharpWeb;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
            X = 0,
            Width = new((e)=>e.ParentBlock.Width),

            PaddingTop = 20,
            //MarginLeft = 8,
            PaddingLeft = new((e) => e.ParentBlock.PaddingLeft),
            PaddingRight = new((e) => e.ParentBlock.PaddingRight),


            Children = {
                new Func<IBlock>(()=>{ 
                    var result = new Column{
                        new Blocks(){
                            H1("Links"),
                            "тут будут ссылки"
                        }.Select(x=>{
                            return x;
                        })
                    };
                    return result;
                })(),

                new Column{
                    new Blocks(){
                        H1("Links"),
                        "тут будут ссылки"
                    }.Select(x=>{
                        return x;
                    })                    
                },
                new Column{
                    MarginTop = 20,
                    Children = {
                        H1("Column 2"),
                        "тут будут еще ссылки"
                    }
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
