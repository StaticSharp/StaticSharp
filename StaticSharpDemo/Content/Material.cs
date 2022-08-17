using StaticSharp.Gears;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StaticSharpDemo.Root {
    public partial class Material : StaticSharp.Material {

        protected override void SetupBundings(MaterialBindings<MaterialJs> bindings) {
            bindings.ContentWidth = e => 960;// (e.WindowWidth > 960) ? 960 : 640;
        }
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
                        Between = e=>1
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
                X = e=> -e.ParentBlock.MarginLeft,
                Width = e=>e.ParentBlock.Width + e.ParentBlock.MarginLeft + e.ParentBlock.MarginRight,

                PaddingTop = e=>20,
                PaddingBottom = e=>20,

                PaddingLeft = e => e.ParentBlock.MarginLeft,
                PaddingRight = e => e.ParentBlock.MarginRight,
            },

            Children = {

                new Blocks(){
                    new Space(float.Epsilon,1,float.Epsilon),
                    new Column{
                        FooterTitle("Links"),
                        "тут будут ссылки"
                    },
                    new Space(float.Epsilon,1,float.Epsilon),
                    new Column{
                        FooterTitle("Column 2"),
                        "тут будут еще ссылки"

                    },
                    new Space(float.Epsilon,1,float.Epsilon),
                    new Column{
                        FooterTitle("Column 3"),
                        "и тут будут ссылки",
                        "line 2",
                        "line 3"
                    },
                    new Space(float.Epsilon,1,float.Epsilon),


                }.Modify(x=>{
                    foreach (var c in x.Values.OfType<Column>()){

                        c.Bindings.MarginLeft = e=> 10;
                        c.Bindings.MarginRight = e=> 10;
                        c.Bindings.MarginTop = e=> 20;
                        c.Bindings.MarginBottom = e=> 20;

                        c.Children.Values.OfType<Block>().First().Modify(x=>{
                            x.Bindings.MarginTop = e=> 0;
                        });

                        c.Children.Values.OfType<Block>().Last().Modify(x=>{
                            x.Bindings.MarginBottom = e=> 0;
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
