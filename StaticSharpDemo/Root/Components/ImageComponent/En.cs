

using System;
using System.Linq;

namespace StaticSharpDemo.Root.Components.ImageComponent {


    abstract partial class Common : Page {
        public Image ImageExample => new Image("ImageExample1.psd");
    }


    [Representative]
    partial class En : Common {
        public override Inlines Description => $"StaticSharp Image component.";

        public override Block? MainVisual => new Image("ImageExample1.jpg");

        

        public override Blocks? Content => new() {


            "Hot reload",
            "Native formats jpg, png, svg",
            "Atuo convertions psd->jpg:80",
            "Manual convertions and resize",
            "Thumbnails",

            H2("Psd format support"),
            $"Psd convertion is supported using {new Uri("https://github.com/dlemstra/Magick.NET"):Magick.NET} library.",
            new Image("ImageExample1.psd"){ 
                Embed = Image.TEmbed.Thumbnail
            },



        };
    }
}
