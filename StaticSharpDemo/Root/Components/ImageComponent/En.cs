

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

            SectionHeader("Psd format support"),
            $"Psd convertion is supported using {new Uri("https://github.com/dlemstra/Magick.NET"):Magick.NET} library.",
            new Image("ImageExample1.psd"){ 
                Embed = Image.TEmbed.Thumbnail
            },


            SectionHeader("Clipping"),
            new Image("ImageExample1.psd"){
                Fit = Fit.Outside,
                Embed = Image.TEmbed.Thumbnail,
                Height = new(e=>e.Width),
                Margins = new(e=>e.AsBoxShadow().Spread),
                ClipChildren = true,
                Modifiers = { 
                    new BorderRadius{ 
                        RadiusBottomLeft = new(e=>e.AsBlock().Height * 0.5)
                    },
                    new BoxShadow{ 
                        Spread = 2,
                        Color = Color.DarkGreen
                    }
                }
            },




        };
    }
}
