namespace StaticSharpDemo.Root.Components.CodeComponent {

    [Representative]
    public partial class En : ComponentPage {
        public override Inlines DescriptionContent => $"CodeBlock and CodeInline component.";
        //public override Block? MainVisual => new Image("ImageExample1.jpg");
        public override Blocks? Content => new() {

            $"Interpolated strings are not supported in {new CodeInline(nameof(CodeBlock))}",
            new ScrollLayout{
                Height = new(e=>Js.Math.Min(e.Content.InternalHeight,600)),
                Content = 
                    new CodeBlock(ThisFileName(),"cs"){
                        Paddings = 10,
                        //BackgroundColor = Color.LightPink,//Custom Color type needed
                    }
            },
        };
    }
}