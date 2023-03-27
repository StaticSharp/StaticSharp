

using StaticSharp.Tree;
using System.Linq;

namespace StaticSharpDemo.Root.Components.CodeComponent {

    [Representative]
    public partial class En : Page {
        public override Inlines Description => $"CodeBlock and CodeInline component.";
        //public override Block? MainVisual => new Image("ImageExample1.jpg");

        



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





        };
    }
}