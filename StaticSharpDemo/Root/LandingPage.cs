using StaticSharp.Gears;
using System.Linq;

namespace StaticSharpDemo.Root {
    public abstract partial class LandingPage: StaticSharp.Page {
        public override string? SiteName => "StaticSharp";
        public override Genome<IAsset>? Favicon => LoadFile("https://raw.githubusercontent.com/StaticSharp/StaticSharpBrandAssets/main/FavIcon.svg");

        public override string PageLanguage => Node.Language.ToString().ToLower();

        public virtual Block LanguageMenu => new Row {
            Children = {
                VirtualNode.GetAllParallelNodes().Select(
                    x=>new Paragraph{ 
                        Inlines = {
                            new Inline(x.Language.ToString()){
                                InternalLink = x
                            }
                        }
                    }
                )
            }
        };


        public Blocks SocialLinks => new() {
            new SvgIconBlock(Icons.SimpleIcons.GitHub){
                ExternalLink = "https://www.github.com/staticsharp"
            },
            new SvgIconBlock(Icons.SimpleIcons.Facebook){
                ExternalLink = "https://www.facebook.com/staticsharp"
            },
            new SvgIconBlock(Icons.SimpleIcons.Twitter){
                ExternalLink = "https://www.twitter.com/staticsharp"
            },
            new SvgIconBlock(Icons.SimpleIcons.Discord){
                ExternalLink = "https://www.twitter.com/staticsharp"
            },
        };


        public virtual Block Menu => new Row {
            BackgroundColor = Color.White,
            Children = {
                new Image("https://raw.githubusercontent.com/StaticSharp/StaticSharpBrandAssets/main/LogoHorizontal.svg") {
                    Embed = Image.TEmbed.Image,
                    Height = 32,
                    MarginsVertical = 6,
                    MarginsHorizontal = 20,
                    
                },
                new Space(),
                SocialLinks.Modify(x=>{
                    foreach (var i in x.OfType<SvgIconBlock>()){
                        i.Margins = 10;
                    }
                }),
                MenuItem(Node.Components),
            },
            
        }.FillWidth().InheritHorizontalPaddings();

        public abstract Blocks? Content { get; }

        public virtual double ColumnWidth => 1080;

        protected override Blocks BodyContent => new Blocks {
            new ScrollLayout { 
                ScrollY = new(e=>Js.Storage.Restore("MainScroll", () => e.ScrollYActual)),
                //FontSize = new(e =>Js.Math.First( Js.Storage.Restore("FontSize", () => 10)),
                Content = new Column{
                    Width = new(e=>e.ParentBlock.Width),
                    PaddingsHorizontal = new(e=>Js.Math.Max(e.ParentBlock.Width-ColumnWidth , 0)/2),
                    Children = {
                        Menu,
                        Content
                    }                
                }.CenterHorizontally()           
            }.FillWidth().FillHeight()           
        };
    }
}
