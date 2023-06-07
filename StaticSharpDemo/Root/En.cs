using Newtonsoft.Json.Linq;
using StaticSharp.Gears;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StaticSharpDemo.Root {

    [Representative]
    partial class En : Page {
        public override string Title => "StaticSharp";
        public override Inlines Description => $"Component oriented static-site generator\nextendable with C#";



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
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") {

            return new Paragraph(text, callerLineNumber, callerFilePath) {
                //ForegroundColor = Color.FromGrayscale(0.4),
                LetterSpacing = 0.02,
                FontSize = 50,
                MarginTop = 80,
                Weight = FontWeight.ExtraLight,

                //FontStyle = new FontStyle(fontWeight),
            };
        }




        /*public static Inline Code(string text, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {

            return new Inline(callerLineNumber, callerFilePath) {
                PaddingsHorizontal = 0.25,
                PaddingBottom = 0.25,
                PaddingTop = 0.1,
                Radius = 4,
                Weight = FontWeight.Regular,
                ForegroundColor = Color.FromGrayscale(0.3),
                BackgroundColor = Color.FromGrayscale(0.90),
                FontFamilies = { new FontFamilyGenome("Roboto Mono") },
                Children = {
                    new Text(text, true, callerLineNumber, callerFilePath)
                }
            };
        }*/


        Block MakeHeader(Image image, Block block) {

            var vertical = new Property<JBlock, bool>(
                e=>e.Width<800
                );

            var imageHeightVertical = image.CreateProperty(e => 500);


            //Property<JImage, double> imageHeightVertical = new(e=>e.)




            Property<JBlock, double> maxHeight = new (
                e => Js.Num.Max(image.Front.AsBlock().Height)
                );



            var result = new Block {
                Height = new(e => vertical.Front ? 1200: image.Front.AsBlock().Height),
                BackgroundColor = Color.Violet,
                UnmanagedChildren = {
                    image.Modify(x=>{
                        x.Width = new(e=>vertical.Front ? e.Parent.Width: 0.5*e.Parent.Width );
                    }),
                    block
                }
            };
            result.AttachProperty(vertical);
            result.AttachProperty(maxHeight);
            return result;
        }

        //Block? CreateProperty<T>(Property<JPage>, double>)
        Block Menu2 => new FitView {
            Margins = 30,

            Child = new LinearLayout {                
                Vertical = false,
                ItemGrow = 0,
                GapGrow = 1,
                SecondaryGravity = 0,
                Gap = 20,
                Width = new(e=>Js.Num.Max(e.Parent.Width, e.InternalWidth)),
                Children = {
                    new Image("https://raw.githubusercontent.com/illumetry/IllumetryBrandAssets/master/LogoHorizontalBigText.svg#1"){
                        Width = 250
                    },
                    new Paragraph(){
                        FontSize = 20,
                        Weight = FontWeight.Medium,
                        Inlines = {
                            new Inline("Contact Us".ToUpper()){
                                MarginsHorizontal = 0.5,MarginsVertical= 0.5,
                            },
                            new SvgIconInline(SvgIcons.MaterialDesignIcons.ArrowRight){
                                MarginRight = 0.4,
                            },

                        },
                        NoWrap = true,
                        BackgroundColor = Color.Violet,
                        Paddings = 5,
                        Modifiers = {
                            new BorderRadius{
                                Radius = new(e=>e.AsBlock().Height * 0.5)
                            }
                        }
                    }
                }
            }


        };






        public override Blocks? Content => new() {

            new Paragraph($"STATIC SHARP"){ 
                NoWrap = true,
            }.ToLandingMainHeader(),
            
            System.Linq.Enumerable.Range(0,10).Select(x=>new Image(new JpegGenome(LoadFile($"https://picsum.photos/seed/{x}A/2000/1000")))),

            Description,

            Separator(),

            #region codeExample
            "coDe".ToLandingSectionHeader(Color.Red),

            /*$"This is {new Inline("Red text"){ ForegroundColor = Color.Red}}",
            $"Here is {Bold("bold")} text. This is {Node.Root.ContactUs:internal link}. And an icon : {new SvgIconInline(SvgIcons.SimpleIcons.GitHub)}",
            $"This is compile time error {new Block{ BackgroundColor = Color.Red}}",*/
            "Welcome to StaticSharp! We believe in getting right to the point, so here is the code from this very page.",

            CodeBlockScrollable(LoadFile(ThisFilePath()).GetCodeRegion("codeExample").Highlight(new CSharpHighlighter())),

            Separator(),

            "COMPONENT-based\ncontent development".ToLandingSectionHeader(Color.GreenYellow * 0.75),

            """
            Component-based development is like a Lego set for your website! You get to use pre-made blocks (components) and snap them together, or even create your own custom bricks to build the site of your dreams.
            Plus, it's super scalable and easy to update. Bye-bye, clunky websites - hello, sleek and modern web creations!
            """,

            Separator(),
            
            new Flipper(){
                
                Vertical = new(e=>e.Width<950),
                Reverse = new(e=>e.Vertical),
                MarginLeft = new(e=>e.Parent.PaddingLeft),
                MarginRight = new(e=>e.Parent.PaddingRight),
                MarginsVertical = 75,
                First  = new LinearLayout(){
                    ItemGrow = 0,
                    Width = new(e=>e.Parent.Width * 0.5),
                    MarginLeft = 10,
                    MarginRight = 10,
                    MarginTop = 60,
                    Children = {
                        "copypasteable from\nSTACKOVERFLOW".ToLandingSectionHeader(new Color("#F58025"))
                        ,
                        """
                        Copy-pasteability is the superpower of code - it allows developers to reuse and share code like a boss, saving time and effort in the software development process.
                        No-code or low-code platforms might have their own superpowers, but when it comes to flexibility and customization, code-based approaches reign supreme.
                        So go forth, dear developer, and copy-paste to your heart's content!
                        """,
                    }
                },
                Second = new Image("StackoverflowKeyboard.svg"){                        
                    //Width = new(e=>e.Parent.Width * 0.5),
                    Height = new(e=>Js.Num.Min(e.Width/e.NativeAspect, 400)),
                    Margins = 75,
                    Embed = Image.TEmbed.Image,
                    Fit = Fit.Inside
                }
                
            },

            Separator(),
            "create your own SHORTCUTS".ToLandingSectionHeader(Color.Red),
            """
            For example, on this page, there are colored words in the headings. You can write full formatting in each case
            or you can create a function that highlights all capital letters with a given color and makes all lowercase letters capitalized.
            """,
            CodeBlock("\"create your own SHORTCUTS\".ToLandingSectionHeader(Color.Red)".Highlight(new ColorCodeHighlighter("cs"))),
            $"In this case it is an extension method for type {Code("string")}",

            Separator(),

            "bring it with NUGET".ToLandingSectionHeader(new Color("#004880") * 1.7),
            "All of these shortcuts and components can be wrapped in NuGet packages, so that everyone (including you in the future) can add them to their new site with a few clicks.",

            Separator(),

            "TURING complete text writing".ToLandingSectionHeader(Color.DeepPink),
            $"""
            Yo dawg, we put programming in the text-writing so you can code while you write.
            By the way, did you know that at the time this page was generated on {DateTime.Now.Date.ToString("MMMM dd, yyyy")}, the StaticSharp repository had {
                JObject.Parse(new HttpRequestGenome("https://api.github.com/repos/StaticSharp/StaticSharp").Result.Text).Value<int>("stargazers_count")
                } stars on {
                new Uri("https://github.com/StaticSharp/StaticSharp"):GitHub}?
            """,
            #endregion

            Separator(),

            "AUTOCOMPLETE for everything".ToLandingSectionHeader(Color.Red),
            $"""
            C# is a strongly-typed language, so the IDE has access to information about the available types and their members in compile-time.
            With the introduction of Source Generators in .NET 6, we can even provide IntelliSense for internal links.
            Example: This link -> {Node.Components} is made by {Code("{Node.Components}")} using interpolated string syntax.
            """,
            """
            Modern IDEs are like supercharged text editors, making it a breeze to develop a website using the most powerful framework at your fingertips.
            """,

            Separator(),
            "DEVELOPER mode".ToLandingSectionHeader(Color.Blue),



            new Inlines($"""
            Like any static_site_generator, StaticSharp has a web_server_mode that_allows_you to_see the_site in_a_browser while you work on_it.
            One feature we want to show_of is the source_code_navigation directly from your browser. You can {Code("ctrl+click")} on an element in the browser and StaticSharp will highlight the corresponding line in Visual Studio.
            """).UnderscoreToNbsp(),

            Separator(),
            "HOT-RELOAD for everything".ToLandingSectionHeader(Color.Orange),
            """
            Essentially, your content is part of the code that is executed to create an HTML file. To make changes reflected on the page, the code must be recompiled.
            However, thanks to the hot-reload feature in .NET, such changes are applied instantly and do not require full recompilation.
            """,
            new Paragraph("""
                In addition, StaticSharp in developer mode provides hot reloading of all resources used on the page.
                All images, videos, JavaScript, text files, and code examples will be reloaded when they are modified.
                """){
                MarginTop = 20
            },

            Separator(),

            "any QUESTIONS?".ToLandingSectionHeader(new Color("1a6ed8")),


            new LinearLayout {
                Margins = 4,
                Vertical = new (e => e.Width < 480),
                ItemGrow = 1,                
                Gap = 4,
                Children = {
                    new Blocks{
                        FacebookMessengerButton("staticsharp"),
                        TelegramButton("petr_sevostianov"),
                        DiscordButton("KYF5uneE2V")
                    },
                }
            },
            

        };

        
    }
}
