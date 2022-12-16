using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StaticSharpDemo.Root {


    public static class Styles {

        public static Paragraph SectionHeader(this string text, Color highlightColor, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") { 
            var result = new Paragraph(callerLineNumber, callerFilePath);
            var textFragments = new List<string>();
            var startIndex = 0;
            bool upperCase = true;
            while (startIndex < text.Length) {
                var fragment = string.Concat(text.Skip(startIndex).TakeWhile(x => (x == char.ToLower(x)) ^ upperCase));
                if (fragment.Length > 0) {
                    startIndex += fragment.Length;
                    if (upperCase) {
                        result.Inlines.Add(new Inline(fragment) { ForegroundColor = highlightColor });
                    } else {
                        result.Inlines.Add(fragment.ToUpper());
                    }
                }
                upperCase = !upperCase;
            }
            return result.SectionHeader();
        }

        public static Paragraph SectionHeader(this Paragraph x) {
            x.FontFamilies = new() { "Inter" };
            x.FontSize = 50;
            x.Weight = FontWeight.ExtraLight;
            x.LineHeight = 1.2;
            x.MarginTop = 60;
            return x;
        }

        public static Paragraph MainHeader(this Paragraph x) {
            x = x.SectionHeader();
            x.FontSize = 90;
            return x;
        }

        public static ScrollLayout MakeCodeBlock(this Inlines inlines) {
            return new ScrollLayout {
                Height = new(e => Js.Math.Min(e.InternalHeight, e.Root.Height * 0.8)),
                Radius = 8,
                BackgroundColor = Color.FromGrayscale(0.98),
                Paddings = 20,
                Content = new Paragraph(
                        inlines
                    ) {
                    Weight = FontWeight.Regular,
                    FontFamilies = { "Roboto Mono" }
                },
            };
        }

    }



    [Representative]
    public partial class En : LandingPage {
        public override string Title => "StaticSharp";

        public override Inlines Description => $"Component oriented static-site generator\nextendable with C#";

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

        protected override void Setup(Context context) {
            base.Setup(context);
            FontSize = 18;
            FontFamilies = new() { "Inter" };
            Weight = FontWeight.Light;
        }


        public override Block? MainVisual => new Video("T4TEdzSLyi0") {
            Play = true,
            Mute = true,
            PreferPlatformPlayer = false,
            //Controls = false,
            //Loop = true,
        };



        public static Paragraph LandingHeading(string text,
            //float fontSize,
            //FontWeight fontWeight,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) {

            return new Paragraph(text, callerLineNumber, callerFilePath) {
                //ForegroundColor = Color.FromGrayscale(0.4),
                LetterSpacing = 0.02,
                FontSize = 50,
                MarginTop = 80,
                Weight = FontWeight.ExtraLight,

                //FontStyle = new FontStyle(fontWeight),
            };
        }

        public static Block Separator() {
            return new Block {
                MarginsVertical = 75,
                Height = new(x => 1 / Js.Window.DevicePixelRatio),
                BackgroundColor = new(e => e.ParentBlock.HierarchyForegroundColor)
            }.FillWidth();
        }


        public override Blocks? Content => new() {


            new Row{
                ForegroundColor = Color.Transparent,
                Children = {
                    new Space(),
                    new MaterialDesignIconBlock(MaterialDesignIcons.IconName.Github){
                        ExternalLink = "https://www.github.com/staticsharp"
                    },
                    new MaterialDesignIconBlock(MaterialDesignIcons.IconName.Facebook){
                        ExternalLink = "https://www.facebook.com/staticsharp"
                    },
                    new MaterialDesignIconBlock(MaterialDesignIcons.IconName.Twitter){
                        ExternalLink = "https://www.twitter.com/staticsharp"
                    },
                    new Space(),
                }
            }.Modify(x=>{
                foreach (var i in x.Children.Values.OfType<MaterialDesignIconBlock>()){
                    i.Height = 48;
                    i.Margins = 10;
                    i.StrokeColor = Color.Gray;
                    i.StrokeWidth = new(x=>1 / Js.Window.DevicePixelRatio);
                    
                }
            }),


            

            new Paragraph("STATIC SHARP").MainHeader(),
            Description,

            /*new Paragraph("StaticSharp"){ 
                FontSize = 80,
                LineHeight = 1,
                TextAlignmentHorizontal = TextAlignmentHorizontal.Center,
                FontFamilies = { "Rajdhani" },
                Weight = FontWeight.Thin
            },
            new Paragraph(Description) {
                TextAlignmentHorizontal = TextAlignmentHorizontal.Center,
                FontSize = 24,
                Weight = FontWeight.Light
            },*/
            

            Separator(),

            #region codeExample

            "coDe".SectionHeader(Color.Red),

            "Welcome to StaticSharp! We believe in getting right to the point, so here is the code from this very page.",


            //This is code block:
            LoadFile(ThisFilePath()).GetCodeRegion("codeExample").Highlight().MakeCodeBlock(),


            /*new ScrollLayout {
                Height = new(e=>Js.Math.Min(e.InternalHeight, e.Root.Height * 0.8)),
                Radius = 8,                
                BackgroundColor = Color.FromGrayscale(0.98),
                Paddings = 20,
                Content = new Paragraph(
                        
                    ){                    
                    Weight = FontWeight.Regular,
                    FontFamilies = { "Roboto Mono" }
                },
            }, */           

            Separator(),

            "COMPONENT-based\ncontent development".SectionHeader(Color.GreenYellow * 0.75),

            """
            Component-based development is like a Lego set for your website! You get to use pre-made blocks (components) and snap them together, or even create your own custom bricks to build the site of your dreams.
            Plus, it's super scalable and easy to update. Bye-bye, clunky websites - hello, sleek and modern web creations!
            """,

            Separator(),
            "copypasteable from\nSTACKOVERFLOW".SectionHeader(new Color("#F58025")),
            """
            Copy-pasteability is the superpower of code - it allows developers to reuse and share code like a boss, saving time and effort in the software development process.
            No-code or low-code platforms might have their own superpowers, but when it comes to flexibility and customization, code-based approaches reign supreme.
            So go forth, dear developer, and copy-paste to your heart's content!
            """,

            Separator(),
            "create your own SHORTCUTS".SectionHeader(Color.Red),
            """
            For example, on this page, there are colored words in the headings. You can write full formatting in each case
            or you can create a function that highlights all capital letters with a given color and makes all lowercase letters capitalized.
            """,
            "\"create your own SHORTCUTS\".SectionHeader(Color.Red)".Highlight("cs").MakeCodeBlock(),
            $"In this case it is an extension method for type {Code("string")}",


            Separator(),
            "bring it with NUGET".SectionHeader(new Color("#004880")),
            "All of these shortcuts and components can be wrapped in NuGet packages, so that everyone (including you in the future) can add them to their new site with a few clicks.",

            Separator(),
            "TURING complete text writing".SectionHeader(new Color("#004880")),
            $"""
            Yo dawg, we put programming in the text-writing so you can code while you write.
            Did you know that StaticSharp repo has {JObject.Parse(new HttpRequestGenome("https://api.github.com/repos/StaticSharp/StaticSharp").Result.Text).Value<int>("stargazers_count")} stars on {"https://github.com/StaticSharp/StaticSharp":github}
            
            """,

            #endregion

            /*new Flipper{ 
                First = new Image("Crafting.psd"),
                Second = new Column{ 
                    Children = {
                        H5("Crafting"),
                        "Combine components!!",
                        "Create chortcuts",
                        $"Например на этой странице все заголовки это агрегат {Code("LandingHeader()")}, который внутри выглядит так:"
                    }
                }
            },*/



            LandingHeading("Copy-pasteable from stackoverflow"),
            new Image("CopyPasteable.psd"){ 
                Fit = Fit.Outside,

                Height = new(e=>Js.Math.Min(e.InternalHeight, e.Root.Height * 0.25)),
                /*Children = {
                    new Column(){
                        ForegroundColor = Color.FromGrayscale(0.75),
                        
                        Width = new(e=>e.ParentBlock.Width),
                        Children = { 
                            new Paragraph("Copy-pasteable\nfrom stackoverflow"){ 
                                TextAlignmentHorizontal= TextAlignmentHorizontal.Center,
                                FontSize = 80,
                                Weight = FontWeight.ExtraBold
                            },

                        }
                    }.Center()
                }*/
            },

            LandingHeading("Bring components from Nuget"),
            new Image("NugetLogo.svg"){ 
                Embed = Image.TEmbed.Image,
                Fit= Fit.Inside,
                Height = new(e=>Js.Math.Min(e.InternalHeight, e.Root.Height * 0.25)),
                BackgroundColor = new Color("#002440")                
            }.FillWidth(),




            LandingHeading("Minimized content"),
            "Only used js and fonts included in final page",


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
