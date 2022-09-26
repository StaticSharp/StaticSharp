﻿using StaticSharp.Gears;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharpDemo.Root {
    public partial class Material : StaticSharp.PageSideMenus {

        protected override Task Setup(Context context) {
            ContentWidth = 960;
            return base.Setup(context);
        }

        public override Block LeftSideBar => new Column() {
            BackgroundColor = ColorTranslator.FromHtml("#6d597a"),
            Children = {
                "Menu Item 1",
                "Menu item 2",
                new Space(){
                    Between = 1
                },

                new Paragraph("Social links"){
                    ["Width"] = "() => element.LayoutWidth",
                }
            }
        };


        public static Paragraph FooterTitle(string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            var paragraph = new Paragraph(text, callerFilePath, callerLineNumber) {
                FontSize = 18,
                FontStyle = new FontStyle(FontWeight.Bold)
            };
            return paragraph;
        }

        public override Block? Footer => new Row {

            BackgroundColor = ColorTranslator.FromHtml("#355070"),

            X = new(e => -e.ParentBlock.MarginLeft),

            Width = new(e => e.ParentBlock.Width + e.ParentBlock.MarginLeft + e.ParentBlock.MarginRight),

            PaddingTop = 20,
            PaddingBottom = 20,

            PaddingLeft = new(e => e.ParentBlock.MarginLeft),
            PaddingRight = new(e => e.ParentBlock.MarginRight),


            Children = {

                new Blocks(){
                    new Space(float.Epsilon,1,float.Epsilon),
                    new Column{
                        Children = {
                            FooterTitle("Links"),
                            "тут будут ссылки"
                        }
                    },
                    new Space(float.Epsilon,1,float.Epsilon),
                    new Column{
                        Children = {
                            FooterTitle("Column 2"),
                            "тут будут еще ссылки"
                        }

                    },
                    new Space(float.Epsilon,1,float.Epsilon),
                    new Column{
                        Children = {
                            FooterTitle("Column 3"),
                            "и тут будут ссылки",
                            "line 2",
                            "line 3"
                        }
                    },
                    new Space(float.Epsilon,1,float.Epsilon),


                }.Modify(x=>{
                    foreach (var c in x.Values.OfType<Column>()){

                        c.MarginLeft = 10;
                        c.MarginRight = 10;
                        c.MarginTop = 20;
                        c.MarginBottom = 20;

                        c.Children.Values.OfType<Block>().First().Modify(x=>{
                            x.MarginTop = 0;
                        });

                        c.Children.Values.OfType<Block>().Last().Modify(x=>{
                            x.MarginBottom = 0;
                        });

                        /*foreach (var b in c.Children.Values.OfType<Block>()){
                            b.Bindings.MarginLeft = e=>0;
                            b.Bindings.MarginRight = e=>0;
                        }*/
                    }
                })

            }
        };




    }

}