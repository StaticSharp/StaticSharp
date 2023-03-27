using Newtonsoft.Json.Linq;
using StaticSharp.Gears;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;


namespace StaticSharpDemo.Root {

    [Representative]
    public partial class En : Page {
        public override string Title => "StaticSharp";
        public override Inlines Description => $"Component oriented static-site generator\nextendable with C#";

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

        public static Block Separator(
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            ) {
                return new LayoutOverride
                    {
                        Content = new Block(callerLineNumber, callerFilePath)
                            {
                                MarginsVertical = 75,
                                Height = new(x => 1 / Js.Window.DevicePixelRatio),
                                BackgroundColor = new(e => e.Parent.HierarchyForegroundColor),
                                Visibility = 0.5
                            },
                        OverrideX = new(e => Js.Math.First(e.MarginLeft, 0)),
                        OverrideWidth = new(e => Js.Math.Sum(e.Parent.Width, -e.Content.GetLayer().MarginLeft, -e.Content.GetLayer().MarginRight))
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



        Random Random= new Random(0);


        private class Switch<T> : IVoidEnumerable {

            public Expression<Func<T>> this[Expression<Func<bool>> selector] {
                set {  }
            }

            public static implicit operator T (Switch<T> x) => throw new Exception();
        }


        public override Blocks? Content => new() {


            /*new Image("https://raw.githubusercontent.com/StaticSharp/StaticSharpBrandAssets/main/LogoHorizontal.svg") {
                    Embed = Image.TEmbed.Image,
                    //wi = new(e=>e.Parent.Child<Js.Block>(2).Height - e.MarginTop - e.MarginBottom),
                    Margins = 32,

                },*/

            new MenuResponsive {
                Depth = 1,
                Logo = new Image("https://raw.githubusercontent.com/StaticSharp/StaticSharpBrandAssets/main/LogoHorizontal.svg") {
                    Embed = Image.TEmbed.Image,
                    Height = 32,
                    //MarginsVertical = 6,
                    MarginsHorizontal = 20,

                },
                MenuItems = {
                    MenuItem(Node.Components.ParagraphComponent),
                    MenuItem(Node.Components.ImageComponent),
                    MenuItem(Node.Components.VideoPlayer),
                    MenuItem(Node.Components.ParagraphComponent),
                    MenuItem(Node.Customization.HowToCreateNewComponent)
                },

                Margins = 20
            },

            new Paragraph($"STATIC_SHARP".UnderscoreToNbsp())
            .ToLandingMainHeader(),

            Description,

            Separator(),

            #region codeExample

            "coDe".ToLandingSectionHeader(Color.Red),

            /*$"This is {new Inline("Red text"){ ForegroundColor = Color.Red}}",
            $"Here is {Bold("bold")} text. This is {Node.Root.ContactUs:internal link}. And an icon : {new SvgIconInline(SvgIcons.SimpleIcons.GitHub)}",
            $"This is compile time error {new Block{ BackgroundColor = Color.Red}}",*/
            "Welcome to StaticSharp! We believe in getting right to the point, so here is the code from this very page.",

            CodeBlockScrollable(LoadFile(ThisFilePath()).GetCodeRegion("codeExample").Highlight())
            .Modify(x=>{
                x.Width = new(e=>Js.Math.Min(e.InternalWidth, e.Parent.Width));
            }).CenterHorizontally(),

            Separator(),

            "COMPONENT-based\ncontent development".ToLandingSectionHeader(Color.GreenYellow * 0.75),
            """
            Component-based development is like a Lego set for your website! You get to use pre-made blocks (components) and snap them together, or even create your own custom bricks to build the site of your dreams.
            Plus, it's super scalable and easy to update. Bye-bye, clunky websites - hello, sleek and modern web creations!
            """,

            Separator(),
            Node.Components.ImageComponent.Representative.Description,

            new LinearLayout(){
                Vertical = new(e=>e.Width<950),
                Reverse = new(e=>e.Vertical),
                MarginLeft = new(e=>e.Parent.PaddingLeft),
                MarginRight = new(e=>e.Parent.PaddingRight),
                MarginsVertical = 75,
                PrimaryGravity = null,
                Children = {
                    new LinearLayout(){
                        ItemGrow = 0,
                        Width = new(e=>e.Parent.Width * 0.1),
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
                    new Image("StackoverflowKeyboard.svg"){
                        Width = new(e=>e.Parent.Width * 0.1),
                        Height = 400,
                        Margins = 75,
                        Embed = Image.TEmbed.Image,
                        Fit = Fit.Inside
                    }
                }
            },

            Separator(),
            "create your own SHORTCUTS".ToLandingSectionHeader(Color.Red),
            """
            For example, on this page, there are colored words in the headings. You can write full formatting in each case
            or you can create a function that highlights all capital letters with a given color and makes all lowercase letters capitalized.
            """,
            CodeBlock("\"create your own SHORTCUTS\".ToLandingSectionHeader(Color.Red)".Highlight("cs")),
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
            



            /*new Block{
                ["Height"] = "(e)=>e.Flipped ? Sum(e.First.Height, e.Second.Height) : Max(e.First.Height, e.Second.Height) ",
                BackgroundColor = Color.Pink,
                ["Flipped"] = "()=>element.Width<500",
                
                Children = {
                    {"First",new Paragraph("Title"){ 
                        Width = new(e=>Js.Math.Min(e.Parent.Width * 0.5, 200))
                    }},
                    { "Second", new Paragraph(Description){
                        ["X"] = "(e)=>e.Parent.Flipped ? 0 : e.Parent.First.Width",
                        ["Y"] = "(e)=>e.Parent.Flipped ? e.Parent.First.Height : 0",
                        ["Width"] = "(e)=>e.Parent.Flipped ? e.Parent.Width : e.Parent.Width - e.X",
                        //Width = new(e=>e.Parent.Width - e.X)
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
