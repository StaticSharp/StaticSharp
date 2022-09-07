using StaticSharp.Gears;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StaticSharpDemo.Root {
    public partial class Material : StaticSharp.Material {

        protected override void Setup() {
            base.Setup();
            ContentWidth = 960;// (e.WindowWidth > 960) ? 960 : 640;
        }
        public override IBlock LeftSideBar => new Column() {


            BackgroundColor = Color.FromArgb(255, 0xA0, 0x70, 0x30),


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

        public override IBlock? Footer => new Row {

            BackgroundColor = Color.Black,

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
