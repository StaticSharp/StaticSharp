
using StaticSharp.Gears;

namespace StaticSharpDemo.Root.GettingStarted
{
    partial class GettingStarted : ArticlePage {
        public override Inlines? Description => "Start developing Static Sharp project from scratch";

        public static class Headers {
            public static string CreateNewProject => "Create new project";
            public static string ProjectInVsCodeExtension => "Project in VS Code extension";
            public static string ProjectInCs => "Project in C#";
            public static string AddNewRoute => "Add new route";
            public static string GeneratorMode => "Generator mode";
            public static string VsCodeAlternatives => "VS Code alternatives";
        }

        public override Genome<IAsset> MainVisual => LoadFile("Rocket.svg");

        public override Blocks Article => new() { 
            //$"Updated at {DateTime.Today.ToShortDateString()} (TODO: style)", // TODO: causes constant page reload becuase of different page hash
            SectionHeader("In this article"),
            
            //new LinearLayout {
            //    PaddingLeft = new (e => e.Children.First().MarginLeft + 10),
            //    Children = {
            //        ListItem(SectionHeaderLink(Headers.CreateNewProject)),
            //        ListItem(SectionHeaderLink(Headers.ProjectInVsCodeExtension)),
            //        ListItem(SectionHeaderLink(Headers.ProjectInCs)),
            //        ListItem(SectionHeaderLink(Headers.AddNewRoute)),
            //        ListItem(SectionHeaderLink(Headers.GeneratorMode)),
            //        ListItem(SectionHeaderLink(Headers.VsCodeAlternatives))
            //    }
            //},

            ListItem2(SectionHeaderLink(Headers.CreateNewProject)),
            ListItem2(SectionHeaderLink(Headers.ProjectInVsCodeExtension)),
            ListItem2(SectionHeaderLink(Headers.ProjectInCs)),
            ListItem2(SectionHeaderLink(Headers.AddNewRoute)),
            ListItem2(SectionHeaderLink(Headers.GeneratorMode)),
            ListItem2(SectionHeaderLink(Headers.VsCodeAlternatives)),

            Separator(),

            SectionHeader(Headers.CreateNewProject),

            new Flipper() {
                First = new LinearLayout { // TODO: suppresses marging collapsing!
                    Children = { 
                        "In this article we will explore how to start working with StaticSharp from scratch. The recomended IDE for StaticSharp is Visual Studio Code, so this tutorial will focus on it. Though you can use any other IDE, which we will cover later. ",

                        $@"So first of all let's open VS Code and install ""StaticSharp"" extension. When the extension is installed, you will be able to see a StaticSharp triangle icon in the activity bar (the most left bar). Click it. Now you can see three new views: ROUTES, PAGES and RESOURCES. ROUTES view suggests you to create a new StaticSharp project, while two others are empty. Let's click {Bold("Create new project")} and select a folder and a name for a new project. ",

                        $"In a few seconds new project is created. You can launch it right away just by pressing {Bold("F5")} same as any other dotnet project. Then you can open localhost in browser and see you newly created site. (TODO: maybe open automatically?)" 
                    }
                },
                Second = new Image("1.png") {
                    Height = 300,
                    Margins = 10,
                },
                
                //Vertical = new (e => e.Second.AsImage().Width < 400)
                Vertical = new (e => e.Width < 1000),
                Gap = 30
            },

            Separator(),

            new Flipper() {
                First = new LinearLayout() {
                    Children = {
                        SectionHeader(Headers.ProjectInVsCodeExtension),
                        new Paragraph($@"Next let's have a look how StaicSharp project presented in VS Code extension and what you can do with it. Extension has three views: ROUTES, PAGES and RESOURCES. ROUTES presents a hierarchical sitemap of you site. For a newly created project you can see here a single element ""Root"". If you click on a route you will be able to see  this route pages and resources in corresponding views. A route can have multiple pages, each one serves a single language (corresponding to {FilePath("Language.cs")}).Resources are additional files: images, fonts, as well as any additional C# code for a selected route")
                    },

                    MarginTop = 50
                },

                Second = new Image("4.png") {
                    Height = 500,
                    Margins = 10
                },

                Gap = 30,
                Proportion = 0.7,

                Vertical = new (e => e.Width < 800),
                Reverse = new(e => !e.Vertical)
            },

            Separator(),
            SectionHeader(Headers.ProjectInCs),
            
            new Flipper() {
                First = new Paragraph(
                    $"Now let's put aside StaticSharp extension for a while and go to files explorer. Here you can see that StaticSharp project is just an ordinary dotnet project and all of it's elemets are constructed of well known C# items. Let's go through files briefly. First of all, you can see {FilePath("*.csproj")} file which represents a console application with plugged-in {FilePath("StaticSharp.Core")} nuget package. If you open {FilePath("Program.cs")} you will see quite a concise code, allowing to customize StaticSharp launch, in particular to select one of two main modes: Server or Generator ({FilePath(".vscode/launch.json")} file contains corresponding launch profiles). The crucial item is Root folder, as you might guess it represents a Root route of your web site. All its pages resources, and child routes will be inside. In a default project you can see inside:"
                    ),

                Second = new Image("6.png") {
                    Height = 500,
                    Margins = 10
                },

                Gap = 30,
                Vertical = new (e => e.Width < 800),
            },

            ListItem2($@"{FilePath("Language.cs")} - enum of languages supported by your site, a single language ""En"" by default"),
            ListItem2($"{FilePath("Root.cs")} - a page of Root route. It contains a C# class which defines the page. Every page in StaticSharp is a dedicated class,"),
            ListItem2($"{FilePath("BasePage.cs")} - since page is merely a OOP class, it is possible to have a common ancestor of all you pages to share some code, say define common style,"),
            ListItem2($"{FilePath("FavIcon.svg")}, {FilePath("LogoHorizontal.png")} - optional visual assets, that can be used in Root or possibly other routes"),

            "All folders and files inside route's folder except pages and child routes folders are considered resources of this route and can be found in corresponding view",

            Separator(),
            SectionHeader(Headers.AddNewRoute),

            $@"Let's return to StaticSharp extension and have a look how a new route can be added. Open the extension, right click Root in routes and select ""{Bold("Add child route")}"", enter a name for a new route (e.g. ""FirstArticle"") and select a parent class for a first page of a new route. For an ancestor you always have an option to select ""{Bold("StaticSharp.Page")}"" which is defined in {FilePath("StaticSharp.Core")} package and is an ancestor of all pages. Also, as we've got abstract {Code("BasePage")} defined it is another option to choose (let's select this one for the sake of definiteness). You can define as many abstract base pages as you need, they will be picked up by extension and will be added to suggested options. After you select parent page new route and it's first (and maybe the only) page is created. You can see them in ROUTES and PAGES views. In a text editor you can see a newly created {FilePath("*.cs")} file containting a class of your new page in a namespace corresponding to your new route (file name and path of the page file corresponds to namespace and class name but hierarchy is defined by the latter ones). (TODO:) By default class definition is empty which produces compilation error since parent page has abstract members. Let's fix the error, click a yellow bulb and select ""{Bold("Implement abstract class")}"", three readonly properties appeared: {Code("Content")}, {Code("Title")} and {Code("Description")}. ",
            ListItem2($@"{Code("Content")} is exactly what is displayed on the page. In a simplest case it can be converted from array of strings, so let's make it return {Code(@"new() {""First article content""};")}" ), 
            ListItem2($@"{Code("Title")} is html title, which is displayed in browser tab header. It is a simple string, e.g. {Code(@"""First article title""")}"),
            ListItem2($@"{Code("Description")} is metadata description (TODO:???). It is optional and can just return {Code("null")}. "),
            $@"Now our project compiles without errors, let's launch it. Next go to browser and navigate to ""localhost/firstarticle"" (TODO: firstarticle/en?) and you will see your page. It is a good moment to see benifints of hot reload. Let's change {Code("Title")} or {Code("Content")} property of your new page and just save a file. You will see that changes reflected in a browser in a few moments after file saved.",

            new Image("7.png") {
                Height = 400,
                Margins = 10
            },

            Separator(),
            SectionHeader(Headers.GeneratorMode),

            new Flipper() {
                First = new Paragraph(
                    $@"So far we got familiar with how to create a new StaticSharp project and start working on it, now it's time to have a look how to publish our work. Until now we were launching StaticSharp in server mode, which means that web pages are rendered on the fly, you can actively change them and see result of your edits right away. But the final result we want is a static web site without server, just html and assets. In order to achieve this we need to run StaticSharp in generator mode. In VS Code go to RUN AND DEBUG view and switch launch configuration from ""{Bold("Server")}"" to ""{Bold("Generator")}"". (TODO: have to create folder) Press {Bold("F5")}. StaticSharp will generate a static site and exit. Now your site is located in {FilePath("GeneratedSite")} folder next to {FilePath("*.csproj")}. You could copy it's contents to a simplest web server and it will see your site without any dependencies on dotnet. For example you could install ""Five Server"" extension for VS Code (popular local web server), right click on {FilePath("GeneratedSite")} folder, select ""{Bold("Open with Five Server (root)")}"" and you will see you generated site in a browser. Now change a path in a browser to ""firstartcle/en"" and you will see your newly created page. (TODO: why have to add en??)"),
                Second = new Image("8.png") {
                    Height = 300,
                    Margins = 10
                },

                Proportion = 0.6
            },

            Separator(),
            SectionHeader(Headers.VsCodeAlternatives),

            $@"So we briefly covered a full StaticSharp project lifecycle. Lastly let's say a few words about alternatives. Recomended tool for development is VS Code with StaticSharp extension, though this is not the only approach. The minimal sufficient thing you need to develop StaticSharp project is {FilePath("StaticSharp.Core")} nuget package. So ultimately you could develop your project with just dotnet SDK CLI and any text editor. Or more realistically with any C# friendly IDE. But for now only VS Code has a dedicated extension. Also {Bold("ctrl+click")} navigation (see next articles TODO:) is supported in VS Code and Visual Studio.",

            "To learn more about developing your StaticSharp project see next articles: " +
            "..."

        };
    }
}
