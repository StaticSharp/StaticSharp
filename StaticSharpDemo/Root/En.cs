using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;




namespace StaticSharpDemo.Root {

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
                BackgroundColor = new(e => e.ParentBlock.HierarchyForegroundColor),
                Visibility = 0.5
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


            

            new Paragraph("STATIC SHARP").ToLandingMainHeader(),
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

            "coDe".ToLandingSectionHeader(Color.Red),

            "Welcome to StaticSharp! We believe in getting right to the point, so here is the code from this very page.",


            //This is code block:
            LoadFile(ThisFilePath()).GetCodeRegion("codeExample").Highlight().ToCodeBlock(),


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

            "COMPONENT-based\ncontent development".ToLandingSectionHeader(Color.GreenYellow * 0.75),
            """
            Component-based development is like a Lego set for your website! You get to use pre-made blocks (components) and snap them together, or even create your own custom bricks to build the site of your dreams.
            Plus, it's super scalable and easy to update. Bye-bye, clunky websites - hello, sleek and modern web creations!
            """,


             





            Separator(),
            
            new Flipper() {
                MarginLeft = new (e=>e.ParentBlock.PaddingLeft),
                MarginRight = new (e=>e.ParentBlock.PaddingRight),
                FlipWidth = 800,
                First = new Column(){
                    MarginLeft = 10,
                    MarginRight = 10,

                    Children = {
                        new Space(),
                        "copypasteable from\nSTACKOVERFLOW".ToLandingSectionHeader(new Color("#F58025")),
                        """
                        Copy-pasteability is the superpower of code - it allows developers to reuse and share code like a boss, saving time and effort in the software development process.
                        No-code or low-code platforms might have their own superpowers, but when it comes to flexibility and customization, code-based approaches reign supreme.
                        So go forth, dear developer, and copy-paste to your heart's content!
                        """,
                        new Space(0,2),
                    }
                },
                Second = new Image("CopyPasteable.psd"){
                    Fit = Fit.Outside
                    //Embed = Image.TEmbed.Image,
                }
            },



            Separator(),
            "create your own SHORTCUTS".ToLandingSectionHeader(Color.Red),
            """
            For example, on this page, there are colored words in the headings. You can write full formatting in each case
            or you can create a function that highlights all capital letters with a given color and makes all lowercase letters capitalized.
            """,
            "\"create your own SHORTCUTS\".SectionHeader(Color.Red)".Highlight("cs").ToCodeBlock(),
            $"In this case it is an extension method for type {Code("string")}",


            Separator(),
            "bring it with NUGET".ToLandingSectionHeader(new Color("#004880") * 1.7),
            "All of these shortcuts and components can be wrapped in NuGet packages, so that everyone (including you in the future) can add them to their new site with a few clicks.",

            Separator(),
            "TURING complete text writing".ToLandingSectionHeader(Color.DeepPink),
            $"""
            Yo dawg, we put programming in the text-writing so you can code while you write.
            By the way, did you know that at the time this page was generated on {DateTime.Now.Date.ToString("MMMM dd, yyyy")}, the StaticSharp repository had {JObject.Parse(new HttpRequestGenome("https://api.github.com/repos/StaticSharp/StaticSharp").Result.Text).Value<int>("stargazers_count")} stars on {"https://github.com/StaticSharp/StaticSharp":GitHub}?
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
            """
            Like any static site generator, StaticSharp has a web server mode that allows you to see the site in a browser while you work on it.
            One feature we want to show of is the source code navigation directly from your browser. You can ctrl+click on an element in the browser and StaticSharp will highlight the corresponding line in Visual Studio.
            """,

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

            /*new Block{
                ["Height"] = "(e)=>e.Flipped ? Sum(e.First.Height, e.Second.Height) : Max(e.First.Height, e.Second.Height) ",
                BackgroundColor = Color.Pink,
                ["Flipped"] = "()=>element.Width<500",
                
                Children = {
                    {"First",new Paragraph("Title"){ 
                        Width = new(e=>Js.Math.Min(e.ParentBlock.Width * 0.5, 200))
                    }},
                    { "Second", new Paragraph(Description){
                        ["X"] = "(e)=>e.Parent.Flipped ? 0 : e.Parent.First.Width",
                        ["Y"] = "(e)=>e.Parent.Flipped ? e.Parent.First.Height : 0",
                        ["Width"] = "(e)=>e.Parent.Flipped ? e.Parent.Width : e.Parent.Width - e.X",
                        //Width = new(e=>e.ParentBlock.Width - e.X)
                    } }                
                }            
            },*/

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




        };

        
    }
}
