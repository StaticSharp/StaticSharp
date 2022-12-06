

namespace StaticSharpDemo.Root.Components.ImageComponent {

    [Representative]
    public partial class En : ComponentPage {
        public override Inlines DescriptionContent => $"StaticSharp Image component.";

        public override Block? MainVisual => new Image("ImageExample1.jpg");

        public override Blocks? Content => new() {


            "Hot reload",
            "Native formats jpg, png, svg",
            "Atuo convertions psd->jpg:80",
            "Manual convertions and resize",
            "Thumbnails",

            H2("Psd format support"),
            $"Psd convertion is supported using {"https://github.com/dlemstra/Magick.NET":Magick.NET} library.",
            new Image("ImageExample1.psd")
            


        };
    }
}
