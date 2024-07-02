

using StaticSharp.Tree;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace StaticSharpDemo.Root.Components.Bug {

    [Representative]
    public partial class En : Page {
        public override Inlines Description => null;
        //public override Block? MainVisual => new Image("ImageExample1.jpg");


        public Block ProductCard(string header, string description, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            return new Flipper(callerLineNumber, callerFilePath) {
                Vertical = new(e => !e.Parent.AsLinearLayout().Vertical),
                Width = 100,
                Margins = 0,
                BackgroundColor = Color.DarkGray,
                ForegroundColor = Color.White,

                First = new Block {
                    Height = 100,
                    BackgroundColor = Color.RebeccaPurple,
                },
                Second = new LinearLayout {
                    PrimaryGravity = -1,
                    Paddings = 5,
                    Margins = 0,
                    Children = {

                        new Paragraph(header, callerLineNumber, callerFilePath){
                            FontSize = 20,
                            Margins = 0,
                        },
                        new Paragraph(description, callerLineNumber, callerFilePath){
                            Margins = 0,
                        },
                    }
                }
            };
        }


        public override Blocks? Content => new() {
            ProductCard("Unreal Engine","description"),

            /*new LinearLayout{
                Vertical =  new(e=>e.Root.Width < 800),
                SecondaryGravity = null,
                MarginsHorizontal = 8,
                Gap = 10,

                Children = {
                    
                }

            },*/





        };
    }
}