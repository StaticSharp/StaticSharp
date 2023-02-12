

using StaticSharp.Tree;
using System.Linq;

namespace StaticSharpDemo.Root.Components.CodeComponent {

    [Representative]
    public partial class En : ComponentPage {
        public override Inlines Description => $"CodeBlock and CodeInline component.";
        //public override Block? MainVisual => new Image("ImageExample1.jpg");

        Block PagePreview(Node node) {
            var mainVisual = node.Representative?.MainVisual as Block;
            mainVisual ??= new Block() { BackgroundColor = Color.Gray };
            mainVisual.ClipByParent = true;
            /*if (mainVisual is Image image) {
                image.Aspect = 2;
            }*/
            return new Flipper() {
                MarginsHorizontal = 10,
                MarginsVertical= 10,
                Radius= 10,
                Flipped = new (e=>e.Width < 500),
                InternalLink = node,

                //Height = 200,

                //Width = new(e=>e.LayoutWidth / 2),
                First = mainVisual,
                Second = new Column {
                    Children = {
                        H4(node.Representative.Title),
                        new Paragraph(node.Representative?.Description),
                    }
                },
                Children= {
                    new MaterialShadow {
                        Elevation = 3,
                    },
                }

            };
        }



        public override Blocks? Content => new() {
            /*#region String
            $"Interpolated strings are not supported in {new CodeInline(nameof(CodeBlock))}",
            #endregion
            new ScrollLayout{
                Height = new(e=>Js.Math.Min(e.Content.InternalHeight,600)),
                Content = 
                    new CodeBlock(new CodeRegionGenome(new FileGenome(ThisFileName()),"String")){
                        Paddings = 10,
                        //BackgroundColor = Color.LightPink,//Custom Color type needed
                    }
            },*/

            PagePreview(Node.Parent.ImageComponent),

            PagePreview(Node.Parent.VideoPlayer),

            Enumerable.Range(1,10).Select(x=>{
                return new Block() {
                    Height = 50,
                    Margins = 10,
                    Radius = 3,
                    BackgroundColor= Color.White,
                    Children= {
                        new MaterialShadow {
                            Elevation = new(e=>e.Parent.Hover? 0.5*x : x)
                        }
                    }
                };
            })




        };
    }
}