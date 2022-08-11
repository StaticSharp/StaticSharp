using StaticSharp.Symbolic;
using StaticSharpWeb;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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

  

        public override IBlock? Footer => new Row{
            Bindings = {
                X = e=>0,
                Width = e=>e.ParentBlock.Width,

                PaddingTop = e=>20,
                //MarginLeft = 8,
                PaddingLeft = e => e.ParentBlock.PaddingLeft,
                PaddingRight = e => e.ParentBlock.PaddingRight,
            },

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

                    //result.Children.OfType<Block<>>

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
                    Bindings = {
                        MarginTop = e=> 20,
                    },
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
