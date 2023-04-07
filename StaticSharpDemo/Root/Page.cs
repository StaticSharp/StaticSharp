using System.Linq;
using StaticSharp.Gears;

namespace StaticSharpDemo.Root {
    public abstract partial class Page : StaticSharp.Page {
        public override string? SiteName => "StaticSharp";
        public override Genome<IAsset>? Favicon => LoadFile("https://raw.githubusercontent.com/StaticSharp/StaticSharpBrandAssets/main/FavIcon.svg");

        public override string PageLanguage => Node.Language.ToString().ToLower();

        protected override void Setup(Context context) {
            base.Setup(context);
            FontSize = 18;
            FontFamilies = new() { "Inter" };
            Weight = FontWeight.Light;
            //TextDecorationColor = Color.Blue;
        }


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






        public virtual Blocks LanguageMenuItems => new Blocks {
            VirtualNode.GetAllParallelNodes().Select(
                x=>new Paragraph{
                    Inlines = {
                        new InlineGroup(x.Language.ToString()){
                            InternalLink = x
                        }
                    }
                }
            )            
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


        public virtual Block Menu => new MenuResponsive {
            Depth = 1,
            //HideButton = false,
            //PrimaryGravity = -1,
            //SecondaryGravity = -1,
            Logo = new Image("https://raw.githubusercontent.com/StaticSharp/StaticSharpBrandAssets/main/LogoHorizontal.svg") {
                Embed = Image.TEmbed.Image,
                Height = 32,
                //MarginsVertical = 6,
                MarginsHorizontal = 20,
                InternalLink = VirtualNode.Root,

            },
            Children = {
                    MenuItem(Node.Components),
                    MenuItem(Node.Components.ImageComponent),
                    MenuItem(Node.Components.VideoPlayer),
                    MenuItem(Node.Components.ParagraphComponent),
                    MenuItem(Node.Customization.HowToCreateNewComponent)
                },

            Margins = 20
        };

        public abstract Blocks? Content { get; }

        public virtual double ColumnWidth => 1080;

        Js.Variable<Js.ScrollLayout> MainScrollLayout => new();

        public override Blocks UnmanagedChildren => new Blocks {
            new ScrollLayout {
                Width = new(e=>e.Parent.Width),
                Height = new(e=>e.Parent.Height),
                ScrollY = new(e=>Js.Storage.Restore("MainScroll_"+string.Join("-",VirtualNode.Path), () => e.ScrollYActual)),
                //FontSize = new(e =>Js.Math.First( Js.Storage.Restore("FontSize", () => 10)),
                
                Child = new LinearLayout{
                    Width = new(e=>e.Parent.Width),

                    ItemGrow = 0,
                    GapGrow = 1,
                    Gap = 0,
                    Children = {
                        new LinearLayout{
                            //Width = new(e=>e.Parent.Width),
                            PaddingsHorizontal = new(e=>Js.Math.Max(e.Width-ColumnWidth , 0)/2),
                            Children = {
                                Menu,
                                Content,
                            }
                        },

                        Footer,
                    }
                }
            }.Assign(MainScrollLayout),

            new SvgIconBlock(SvgIcons.MaterialDesignIcons.ArrowUp){ 
                Radius = new(e=>e.Width*0.5),
                Visibility = 0.2,
                BackgroundColor = Color.LightGray,
                Exists = new(e=>MainScrollLayout.Value.ScrollYActual > 100),
                X = new (e=>e.Parent.Width-e.Width - 10),
                Y = new (e=>e.Parent.Height-e.Height - 10),
                Width = 64,
                Height = 64,
            }

        };
    }
}
