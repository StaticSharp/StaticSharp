﻿using StaticSharp.Symbolic;
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
            new Modifier(){

                
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
            }
        };


    }

}
