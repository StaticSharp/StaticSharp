using System.Linq;

namespace StaticSharpDemo.Root {
    public abstract partial class ArticlePage : Page {

        string ViewSourceCodeOfThisPageOnGithub => Node.Language switch {            
            Language.Ru => "Посмотрите исходный код этой страницы на Github",
            _ => "View source code of this page on Github",
        };

        public Paragraph LinkToThisPageSourceCodeOnGithub => new Paragraph(ViewSourceCodeOfThisPageOnGithub) {
            Inlines = { 
                new SvgIconInline(SvgIcons.SimpleIcons.GitHub) { 
                    Scale = 1.5,
                    BaselineOffset = 0.25,
                    MarginLeft = 0.5
                }
            },
            TextAlignmentHorizontal = TextAlignmentHorizontal.Right,
            ExternalLink = "https://github.com/StaticSharp/StaticSharp/tree/main/StaticSharpDemo/Root"
            + string.Concat(VirtualNode.Path.Select(x=>"/"+x))+"/"+ VirtualNode.Representative.GetType().Name + ".cs",
            BackgroundColor = Color.MediumPurple,
            MarginRight = 20,
            Modifiers = { 
                new BorderRadius {
                    //Radius = 4,
                    RadiusBottomRight = new(e=>e.AsBlock().Height / 2),
                    RadiusTopRight = new(e=>e.RadiusBottomRight)
                }
            }
        };

        public override sealed Blocks? Content => new (){
            MainVisual,
            new Paragraph(Title){ 
                Weight = FontWeight.Thin,
                FontSize = 80
            },
            Description,
            LinkToThisPageSourceCodeOnGithub,
            Separator(),
            Article
        };

        public abstract Blocks Article { get; }

    }
}
