using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StaticSharpDemo.Root {



    [Representative]
    public partial class Ru : LandingPage {
        public override string Title => "StaticSharp";

        public override Inlines DescriptionContent => $"Статический генератор сайтов на максималках";

        static string RandomString(int length, Random random) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789     ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        Paragraph CreateParagraph(int numChars, Random random) {
            return new Paragraph(RandomString(numChars, random));
        }

        IEnumerable<Paragraph> CreateParagraphs(int count) {
            Random random = new Random(0);
            return Enumerable.Range(0, count).Select(i => CreateParagraph(50, random));
        }


        public Inline GithubUrl(string text = "GitHub repository") {
            return new Inline {
                ExternalLink = "https://github.com/antilatency/Antilatency.Copilot",
                OpenLinksInANewTab = true,
                ForegroundColor = Color.FromIntRGB(172, 196, 53),
                Children = {
                    text
                }
            };
        }

        public Inline DiscordUrl(string text = "Discord server") {
            return new Inline {
                ExternalLink = "https://discord.gg/ZTqmfPsGEr",
                OpenLinksInANewTab = true,
                ForegroundColor = Color.FromIntRGB(139, 148, 245),
                Children = {
                    text
                }
            };
        }


        /*public override ScrollLayout ScrollLayout => base.ScrollLayout.Modify(x => {
            x.Children.Add(
                new MaterialDesignIconBlock(MaterialDesignIcons.IconName.LanguageCsharp) {
                    Depth = 3,
                    BackgroundColor = Color.White,
                    Height = 80,
                    Radius = new(e => e.Height * 0.5),
                    Paddings = 20,
                    Children = {
                    new LinkBlock("https://en.wikipedia.org/wiki/C_Sharp_(programming_language)").FillHeight().FillWidth()
                }

                }.Center()
                );
        });*/

        public override Block? MainVisual => new Video("T4TEdzSLyi0") {
            Play = true,
            Mute = true,
            PreferPlatformPlayer = false,
            //Controls = false,
            //Loop = true,
        };


        public override Blocks? Content => new() {

            #region header
            new Paragraph("StaticSharp"){ 
                FontSize = 80,
                TextAlignmentHorizontal = TextAlignmentHorizontal.Center,
            },
            new Paragraph(DescriptionContent) {
                TextAlignmentHorizontal = TextAlignmentHorizontal.Center,
            },
            #endregion

            new ScrollLayout {
                Radius = 8,
                BackgroundColor = Color.FromGrayscale(0.8),
                Margins = 0,
                Paddings = 0,
                Content = new Paragraph(
                        LoadFile(ThisFilePath()).GetCodeRegion("header").Highlight()
                    ){
                    PaddingsHorizontal = 20,
                    Weight = FontWeight.Regular,
                    FontFamilies = new FontFamilyGenome[]{ 
                        new FontFamilyGenome("Roboto Mono")
                    }
                }
            },

            $"{new CheckboxInline(){ Value = new(e=>Js.Storage.Restore("check",()=>e.ValueActual))}}",



            new Column(){
                BackgroundColor = Color.FromGrayscale(0.15),
                Children = {
                    $"Refer to {GithubUrl()} for more information, and join our {DiscordUrl()} to learn more about getting early access to Copilot.",
                    new Flipper() {
                        MarginLeft = new (e=>e.ParentBlock.PaddingLeft),
                        MarginRight = new (e=>e.ParentBlock.PaddingRight),

                        First = new Column(){
                            MarginLeft = 10,
                            MarginRight = 10,

                            Children = {
                                new Space(),
                                H4("Antilatency Copilot.\nPositional solution for drones").Modify(x=>{
                                    x.LineHeight = 1.3f;
                                }),
                                "Copilot is an Antilatency project. We use our accurate optical-inertial tracking system with Raspberry Pi to provide you with precise indoor navigation and outdoor landing for drones in different use cases.",
                                new Space(0,2),
                            }
                        },
                        Second = new Image("Copilot/SchemeDark.svg"){
                            Embed = Image.TEmbed.Image,
                        },
                        Children = { 
                            new MaterialDesignIconBlock(MaterialDesignIcons.IconName.Github){
                                ExternalLink = "https://github.com/staticsharp",
                                Width = 128,
                                BackgroundColor = Color.White,
                                Radius = new(e=>e.Width /2),
                                Paddings = 12,
                            }.Center()
                        }
                    }
                }

            }.FillWidth().InheritHorizontalPaddings(),

            new Flipper(){
                First = new Image("Copilot/Delivery.svg"){
                    Embed = Image.TEmbed.Image,
                    MarginLeft = 24,
                    MarginRight = 24,
                    MarginTop = 24,
                    MarginBottom = 24,
                },
                Second = new Column(){
                    MarginLeft = 10,
                    MarginRight = 10,

                    Children ={
                        new Space(),                        
                        //$"{new Checkbox():#id}",
                        H5("Increased delivery efficiency"),
                        "Autonomous landing at delivery points, automatic delivery scenarios in large warehouses and enterprises.",
                        new Space(),
                    }
                }
            }.FillWidth().InheritHorizontalPaddings(),

            "This product is still under development, but you can get early access to Copilot, join discussions, and share your ideas in our community on Discord",


            /*new Image(new FileGenome(AbsolutePath("TestPsdImage.psd"))),

            new Template(new FileGenome(AbsolutePath("Reactive.template"))),

            H1($"H1"),
            "Abc",
            "Abc",*/
            

            /*H1("H1"),*/
            /*new Modifier {
                FontSize = new((element) => element.Sibling<SliderJs>("Slider").Value + 2),
                Children = { $"Modifier test" }
            },*/

            //CreateParagraphs(100),

            {
                "Slider",
                new Slider {
                    
                    //MarginTop = e=>e.Value,
                    Min = 10,
                    Max = 200
                }
            },

            /*$"A B C D {4}",
            new Space(),

            $"Bold: {new InlineModifier { FontStyle = new FontStyle { Weight = FontWeight.Bold }, Children = { "Text" } }}",
            $"Если   понадобится компонент,которого нет среди стандартных.",
            $"Можно создать компонент прям в проекте вашего сайта."*/
        };

        
    }
}
