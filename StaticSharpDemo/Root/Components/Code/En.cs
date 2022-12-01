

using StaticSharp.Tree;

namespace StaticSharpDemo.Root.Components.CodeComponent {

    [Representative]
    public partial class En : ComponentPage {
        public override Inlines DescriptionContent => $"CodeBlock and CodeInline component.";
        //public override Block? MainVisual => new Image("ImageExample1.jpg");

        Block PagePreview(Node node) {
            var mainVisual = node.Representative?.MainVisual as Block;

            return new Flipper() {
                MarginsHorizontal = 10,
                MarginsVertical= 10,
                Radius= 10,
                BackgroundColor= Color.Red,
                FlipWidth = 500,
                //Width = new(e=>e.LayoutWidth / 2),
                First = mainVisual ?? new Block() { BackgroundColor = Color.Red },
                Second = new Column {
                    Children = {
                        H4(node.Representative.Title),
                        new Paragraph(node.Representative?.DescriptionContent),
                    }
                }

            };
        }



        public override Blocks? Content => new() {
            #region String
            $"Interpolated strings are not supported in {new CodeInline(nameof(CodeBlock))}",
            #endregion
            new ScrollLayout{
                Height = new(e=>Js.Math.Min(e.Content.InternalHeight,600)),
                Content = 
                    new CodeBlock(new CodeRegionGenome(new FileGenome(ThisFileName()),"String")){
                        Paddings = 10,
                        //BackgroundColor = Color.LightPink,//Custom Color type needed
                    }
            },

            PagePreview(Node.Parent.ImageComponent),

            new Flipper(){
                FlipWidth = 500,
                //Width = new(e=>e.LayoutWidth / 2),
                First = Node.Representative.MainVisual ?? new Block(){BackgroundColor = Color.Red},
                Second = new Column{ 
                    Children = { 
                        H4(Node.Representative.Title),
                        Node.Representative.Description,
                    }
                },
                Children = { 
                    new MaterialShadow()


                }
                


            }

        };
    }
}