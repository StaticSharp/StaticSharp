using System.Linq;
using StaticSharp.Gears;
using StaticSharp.Js;

namespace StaticSharpDemo.Root {
    public abstract partial class Page : PageBase {
        public override string? SiteName => "StaticSharp";
        public override Genome<IAsset>? Favicon => LoadFile("FavIcon.svg");

        public override string PageLanguage => Node.Language.ToString().ToLower();

        protected override void Setup(Context context) {
            base.Setup(context);
            FontSize = new (e=>Js.Num.PiecewiseLinearInterpolation(
                Js.Window.DevicePixelRatio,
                new(1, 18),
                new(2, 16)
                )
            );
            FontFamilies = new() { "Inter" };
            Weight = FontWeight.Light;
            //TextDecorationColor = Color.Blue;
        }

        

        public override Block? MainVisual => null;
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
                        new Inline(x.Language.ToString()){
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
            PaddingsHorizontal = new(e => Js.Num.Max(e.Parent.Width - ColumnWidth, 0) / 2),
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
                $"This website has been created using {Node.Root}"
            }
        };

        public Genome<IAsset> LogoGenome => LoadFile("https://raw.githubusercontent.com/StaticSharp/StaticSharpBrandAssets/main/LogoHorizontal.svg");

        public virtual Block Menu => new MenuResponsive {
            Depth = 1,
            //HideButton = false,
            //PrimaryGravity = -1,
            //SecondaryGravity = -1,
            Logo = new Image(LogoGenome) {
                Embed = Image.TEmbed.Image,
                Height = 32,
                //MarginsVertical = 6,
                MarginsHorizontal = 20,
                InternalLink = VirtualNode.Root,

            },
            Children = {
                    MenuItem(Node.Components),
                    MenuItem(Node.Modifiers),
                    MenuItem(Node.Components.VideoPlayer),
                    MenuItem(Node.Components.ParagraphComponent),
                    MenuItem(Node.Customization.HowToCreateNewComponent),
                    new LinearLayout(){ 
                        PrimaryGravity = 0,
                        Vertical = false,
                        Children = {
                            LanguageMenuItems
                        }
                    }
                },

            Margins = 20
        };

        public abstract Blocks? Content { get; }

        public virtual double ColumnWidth => 1080;

        Js.Variable<JScrollLayout> MainScrollLayout => new();

        


        public override Blocks UnmanagedChildren => new Blocks {


            new SvgIconBlock(SvgIcons.MaterialDesignIcons.ChevronDoubleUp){
                Depth= 1,         
                Visibility = new(e=>Animation.SpeedLimit(1, e.AsHover().Value ? 0.5 : 0.2)) ,
                BackgroundColor = Color.LightGray,
                Exists = new(e=>MainScrollLayout.Value.ScrollY > 100),
                X = new (e=>e.Parent.Width-e.Width - 10),
                Y = new (e=>e.Parent.Height-e.Height - 10),
                Width = 64,
                Height = 64,
                Modifiers = {
                    new Button{
                        Script = $"{MainScrollLayout.Name}.ScrollY = 0"
                    },
                    new Cursor(CursorOption.Pointer),
                    new Hover(),
                    new BorderRadius(){
                        Radius = new(e=>e.AsBlock().Width*0.25),
                    }
                }
            },


            new ScrollLayout {
                Width = new(e=>e.Parent.Width),
                Height = new(e=>e.Parent.Height),
                
                Modifiers = { 
                    new SessionStorageNumber{ 
                        Name = "MainScroll_"+string.Join("-",VirtualNode.Path),
                        ValueToStore = new(e=>e.AsScrollLayout().ScrollY)
                    }.Assign(out var MainScrollLayoutPosition)
                },
                ScrollY = new(e=>MainScrollLayoutPosition.Value.StoredValue),


                Child = new LinearLayout{
                    
                    Width = new(e=>e.Parent.Width),

                    ItemGrow = 0,
                    GapGrow = 1,
                    Gap = 50,
                    Children = {
                        new LinearLayout{
                            
                            //Width = new(e=>e.Parent.Width),
                            PaddingsHorizontal = new(e=>Js.Num.Max(e.Width-ColumnWidth , 0)/2),
                            Children = {
                                Menu,
                                Content,
                            }
                        },

                        Footer,
                    }
                }
            }.Assign(MainScrollLayout),

            

        };
    }
}
