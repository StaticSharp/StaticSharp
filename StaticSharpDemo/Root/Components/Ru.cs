

using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpDemo.Root.Components {

    abstract partial class Common : Page {
        /*public Block PagePreview(StaticSharp.Tree.Node node) {
            var mainVisual = node.Representative?.MainVisual as Block;
            mainVisual ??= new Block() { BackgroundColor = Color.Gray };
            mainVisual.ClipByParent = true;
            return new Flipper() {
                MarginsHorizontal = 10,
                MarginsVertical = 10,
                Radius = 10,
                Flipped = new(e => e.Width < 500),
                InternalLink = node,

                //Height = 200,

                //Width = new(e=>e.LayoutWidth / 2),
                First = mainVisual,
                Second = new LinearLayout {
                    Children = {
                        H4(node.Representative.Title),
                        new Paragraph(node.Representative?.Description),
                    }
                },
                Children = {
                    new MaterialShadow {
                        Elevation = 3,
                    },
                }

            };
        }*/
    }

        [Representative]
    partial class Ru : Common {
 


        public override Inlines Description => $"Компоненты для создания страниц.";



        



        public override Blocks Content => new(){

           // Node.Children.Select(x=>PagePreview(x)),


            Node.Children.Select(x=>new LinearLayout{ 
                InternalLink = x,
                BackgroundColor = new(e=>e.AsHover().Value ? Color.FromGrayscale(0.95) : Color.White),
                Modifiers = { 
                    new Hover()
                },

                Children = {
                    H4(x.Representative?.Title),
                    x.Representative?.Description
                }
            }),
        };

    }
}

