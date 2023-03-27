using StaticSharp.Gears;
using System.Linq;

namespace StaticSharpDemo.Root {
    public abstract partial class Page : StaticSharp.Page {
        public override string? SiteName => "StaticSharp";
        public override Genome<IAsset>? Favicon => LoadFile("https://raw.githubusercontent.com/StaticSharp/StaticSharpBrandAssets/main/FavIcon.svg");

        public override string PageLanguage => Node.Language.ToString().ToLower();

        public override object? MainVisual => null;
        public override string Title {
            get {
                var result = GetType().Namespace;
                result = result[(result.LastIndexOf('.') + 1)..];
                result = StaticSharp.Gears.CaseUtils.CamelCaseRegex.Replace(result, match => {
                    if (match.Index == 0) return match.Value;
                    return " " + match.Value;
                });

                return result;
            }
        }


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
            new SvgIconBlock(SvgIcons.SimpleIcons.GitHub){
                ExternalLink = "https://www.github.com/staticsharp"
            },
            new SvgIconBlock(SvgIcons.SimpleIcons.Facebook){
                ExternalLink = "https://www.facebook.com/staticsharp"
            },
            new SvgIconBlock(SvgIcons.SimpleIcons.Twitter){
                ExternalLink = "https://www.twitter.com/staticsharp"
            },
            new SvgIconBlock(SvgIcons.SimpleIcons.Discord){
                ExternalLink = "https://www.twitter.com/staticsharp"
            },
        };

        public virtual Block Footer => new LinearLayout {
            FontSize = 14,
            PaddingsHorizontal = new(e => Js.Math.Max(e.Parent.Width - ColumnWidth, 0) / 2),
            BackgroundColor = Color.FromGrayscale(0.9),
            Children = { 
                /*new LinearLayout {
                    Vertical = false,
                    
                    Children = { 
                        "A",
                        "B",
                        "C"
                    }
                },*/
                $"This website is created using {Node.Root}"
            }
        };


        public virtual Block Menu => new Row {
            Children = {
                new Image("https://raw.githubusercontent.com/StaticSharp/StaticSharpBrandAssets/main/LogoHorizontal.svg") {
                    Embed = Image.TEmbed.Image,
                    Height = 32,
                    MarginsVertical = 6,
                    MarginsHorizontal = 20,

                },
                    SocialLinks.Modify(x=>{
                        foreach (var i in x.OfType<SvgIconBlock>()){
                            i.Paddings = 10;
                        }
                    })
                }
        }.FillWidth().InheritHorizontalPaddings();

        public abstract Blocks? Content { get; }

        public virtual double ColumnWidth => 1080;

        public override Blocks Children => new Blocks { // for top level element Width must be set/overriden (e.g. FillWidth), otherwise - recursive binding
            new ScrollLayout {
                Width = new(e=>e.Parent.Width),
                Height = new(e=>e.Parent.Height),
                ScrollY = new(e=>Js.Storage.Restore("MainScroll", () => e.ScrollYActual)),
                //FontSize = new(e =>Js.Math.First( Js.Storage.Restore("FontSize", () => 10)),
                
                Content = new LinearLayout{
                    Width = new(e=>e.Parent.Width),

                    ItemGrow = 0,
                    GapGrow = 1,
                    Gap = 50,
                    Children = {
                        new LinearLayout{
                            PaddingsHorizontal = new(e=>Js.Math.Max(e.Parent.Width-ColumnWidth , 0)/2),
                            Children = {
                                Menu,
                                Content,
                            }
                        }.FillWidth(),

                        Footer,
                    }
                }

            }
        };
    }
}
