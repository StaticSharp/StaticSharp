
using StaticSharp;

namespace StaticSharpDemo.Root.CreatingSite {

    public partial class CreatingSite : StaticSharpDemo.Root.ArticlePage {
        public override Inlines? Description => "Explore StaticSharp web development";

        // public override Block? MainVisual => //TODO:

        // TODO: idea: make RoutingSG generate list of anchors for a page
        public static class Headers {
            public static string InThisArticle => "In this article";

            public static string Components => "Components";
            
            public static string PageAsComponent => "Page as a component";

            public static string PageLayout => "Page layout";

            public static string LayoutComponentsLinearLayout => "Layout components: LinearLayout";
            
            public static string MarginsAndPaddings => "Margins and paddings";

            public static string Bindings => "Bindings";
            
            public static string LayoutComonentsScrollView => "Layout comonents: ScrollView";

            public static string Navigation => "Navigation"; //  TODO: add modifiers -> Hover, visited, cursor:pointer?
            
            public static string PageMetadata => "Page metadata";
        }


        public override Blocks Article => new() { 
            
            SectionHeader(Headers.InThisArticle),
            #region InThisArticle

            new LinearLayout {
                PaddingLeft = new (e => e.Children.First().MarginLeft + 10),
                Children = {
                    ListItem(SectionHeaderLink(Headers.Components)),
                    ListItem(SectionHeaderLink(Headers.PageAsComponent)),
                    ListItem(SectionHeaderLink(Headers.PageLayout)),
                    ListItem(SectionHeaderLink(Headers.LayoutComponentsLinearLayout)),
                    ListItem(SectionHeaderLink(Headers.MarginsAndPaddings)),
                    ListItem(SectionHeaderLink(Headers.Bindings)),
                    ListItem(SectionHeaderLink(Headers.LayoutComonentsScrollView)),
                    ListItem(SectionHeaderLink(Headers.Navigation)),
                    ListItem(SectionHeaderLink(Headers.PageMetadata)),
                }
            },

            $@"In this article we will explore how to do a common web development tasks with StaticSharp. As an example we will develop a simple business site for a web design studio. (This article implies that you have a basic idea of StaticSharp and also managed to install it and launch StaticSharp project. If not, please have a look at {Node.Root.GettingStarted} ). All below implies StaticSharp works in a server mode since now we concentrate on development not publishing. Full sample project, created in this article can be found {new Inline("here"){ ExternalLink = "https://github.com/pavel-zybenkov/MyWebStudio", OpenLinksInANewTab = true }}",
            Separator(),
            #endregion

            SectionHeader(Headers.Components),
            #region Components
            $@"So first of all let's open VS Code, create a StaticSharp project, name it ""MyWebStudio"" and start sculpting our first page. Pay attention where main content is placed. It is a read-only property {Code("public override Blocks Content")}. The property has type {Code("Blocks")}. {Code("Blocks")} is one of SaticSharp components. (TODO: how to define components? Descendants of BaseModifier?) Components are fundamental building blocks of StaticSharp pages. Component instance is eventually transformed into an DOM element and managed by a dedicated javascript object.",
            
            $@"StaticSharp provides a list of basic components, which are described in {Node.Root.Components} page. If basic components are not enough you can develop your own component following instruction provided in {Node.Root.Customization.HowToCreateNewComponent} page. Also every page itself is a component (each page is a different one).",

            $@"Each component has a corresponding .NET class which is used to create an instance of a component (instance of a class), configure it (set up instance properties) and place it on the page (set some page property to return a configured instance of a component class).",

            $@"Let's have a closer look at {Code("Content")} property and modify it. {Code("Root")} is a page which is a component. It has a readonly property {Code("Content")} which returns an instance of {Code("Blocks")} component. Since {Code("Blocks")} implements {Code("IEnumerable")} it is possible to initialize it with comma separated enumerable elements in curly brackets. Let's change the default single string element",
            CodeBlock(""""""
            public override Blocks Content => new() {
                """
                Text text text text text text text text text text text text text
                text text text text text text text text text text text text text.
                """
            };
            """"""
            .Highlight(new CSharpHighlighter())),

            "to a more comprehensive content:",
            CodeBlock("""
            public override Blocks Content => new() {
                new Image("MyWebStudioLogo.png"){
                    Width = 500
                },
                new Paragraph("MY web studio") {
                    FontSize = 32,
                    ForegroundColor = Color.GreenYellow,
                },
                "We create the best web sites in the world!"
            };
            """
            .Highlight(new CSharpHighlighter())),

            $@"This code contains a reference to an image file. You can download it from here. TODO:",

            // TODO: how to make image 100% height and width?

            $@"Let's examine what is wrtten here. {Code("Blocks")} is initialized with a collection of components. {Code("Image")} is a component, which allows to put image assets on the page (like {Code("<img>")} html tag). Paragraph is another component wich represents a block of text (like {Code("<p>")}). Note that {Code("Paragraph")}, like other components, has properties that can be set on component instantiation. The last collection element is a simple string. Does this mean string is also a component? Actually not, but implicitely it is converted into {Code("Paragraph")}, which allows to reduce amount of boilerplate code on the page.",

            // TODO: if image is not found, all fails, have to reload when after fix
            Separator(),
#endregion

            SectionHeader(Headers.PageAsComponent),
            #region PageAsComponent
            $@"After our changes a text and logo looks quite pale on a white background. It would be a logical guess that page as a component has a property - {Code("BackgroundColor")}, which is true. But in case of {Code("Paragraph")} we call it's constructor and initialize properties, while page is a special component which is instantiated by StaticSharp implicetely. So in order to handle page properties we could take two approaches. First - override a virtual property (or implement an abstract property). Second, for cases of non-virtual properties ancestor of all pages, {Code("StaticSharp.Page")}, has a method {Code("Setup")} which is called in it's constructor. So, to change {Code("BackgroundColor")}, we need to add to our page:",

            CodeBlock("""
            protected override void Setup(Context context) {
                BackgroundColor = Color.Gray;
                base.Setup(context);
            }
            """
            .Highlight(new CSharpHighlighter())),

            Separator(),
            #endregion
            
            SectionHeader(Headers.PageLayout),
            #region PageLayout
            $@"OK. Now've got a general idea how to modify content. Next let's lay it out. You might already notice that your {Code("Root")} page class is inherited from {Code("BasePage")} class, which in turn is inherited from {Code("StaticSharp.Page")} - a common ancestor of all pages. You can inherit yout pages from {Code("StaticSharp.Page")} directly, but in order to define common layout and style is it convenient to use the power of OOP and have an interim parent or even multiple ones. If you take a closer look at {Code("Root")} and {Code("BasePage")} pages you will note that the content we are working on eventually is a part of value, returned by {Code("BasePage.UnmanagedChildren")}. {Code("UnmanagedChildren")} property of a page is a starting point of a page layout. It returns a collection of child components which are not handled by parent component in any way, they all are just placed in X=0, Y=0 point of a parent, so when writing your page code you have to take care of their positioning manually, for example by setting {Code("X")} and {Code("Y")} properties to some constant values or binding them to some other properties (which will be discussed later). All the components has {Code("UnmanagedChildren")} property.",

            @$"In our {Code("BasePage")} there is a single unmanaged child {Code(nameof(LinearLayout))} which in turn has not {Code("UnmanagedChildren")} but {Code("Children")} property defined. Not all but a lot of components has {Code("Children")} property. This kind of children are managed by a parent component in a way specific to a particular component. {Code(nameof(LinearLayout))} is designed to place children in a row or in a column. Apart from {Code("LinearLayout")} here is a set of other layout components (TODO: link to a page with list and description) which takes {Code("Children")} or other component-typed properties and lay out their component-values." ,
            Separator(),
            #endregion

            SectionHeader(Headers.LayoutComponentsLinearLayout),
            #region LayoutComponentsLinearLayout
            @$"So, all components you can see in browser (text and logo) are now placed one above the other. And as we found out this is because they all are returned by {Code("Children")} of {Code(nameof(LinearLayout))} inside {Code("BasePage")}. Now, let'say we want text to be placed not below the logo, but to the left side of it. To achieve this we could go to {Code("Root")} page and wrap logo and company name with {Code(nameof(LinearLayout))} with {Code("Vertical=false")} (as an optional improvement let's also set {Code("SecondaryGravity = 0")} to tune vertical alignment):",

            CodeBlock("""
            new LinearLayout{
                Vertical = false,
                SecondaryGravity = 0,
                Children = {
                    new Image("MyWebStudioLogo.png"){
                    Width = 500
                    },
                    new Paragraph("MY web studio") {
                        FontSize = 32,
                        ForegroundColor = Color.GreenYellow,
                    },
                }
            },
            """
            .Highlight(new CSharpHighlighter())),

            $@"So now we've got vertical {Code(nameof(LinearLayout))} having nested horizontal {Code(nameof(LinearLayout))}. Using this technique it is possible to construct really complicated and fine-tuned layouts.",
            $@"Looks like it would look more nice if """" piece of text was right behind company name, to achieve this we should place a nested vertical {Code(nameof(LinearLayout))} in place of compnay name:",

            CodeBlock("""
            new LinearLayout{
                        Children = {
                            new Paragraph("MY web studio") {
                                FontSize = 32,
                                ForegroundColor = Color.GreenYellow,
                            },
                            "We create the best web sites in the world!",
                        }    
                    },
            """
            .Highlight(new CSharpHighlighter())),
            Separator(),
            #endregion

            SectionHeader(Headers.MarginsAndPaddings),
            #region MarginsAndPaddings
            $@"Next thing to improve - lets put page content in a centered column of a fixed width. We can do it with paddings of outermost {Code(nameof(LinearLayout))}. Paddings have to depend on page width (reflecting a browser window width). Since in StaticSharp we are dealing with a programming language not markup we will write an explicit code to calculate paddings in pixels instead of using vague percents or other relative untis. Let's explore how it can be done.",
            "All block components have margins and paddings. Technically, each layout component can take them into account in its special way or ignore them. But all components that are available out-of-the-box treat them in a similarly to html margins and paddings (TODO: with some pecularities, clarify, explain margin collapsing, likely in separate paragraph or article, along with 3-columns pattern and other).",
            Separator(),
            #endregion
            
            SectionHeader(Headers.Bindings),
            #region Bindings
            $@"If you take a closer look at component's class properties, you will notice that they have {Code("Binding<..., ...>")} type. The first generic parameter is specific to a component, it an interface that looks alike to components class. This interface reflects a component's javactipt object and allows to reference it's properties when it is already in browser. The second generic parameter is the resulting type of the property (e.g. for margins and paddings it is {Code("dobule")}, meaning number of pixels). Constructor of {Code("Binding<..., ...>")} can take an expression defining how to calculate a property value from component already rendered in browser (It is also possible to navigate from current componenet to other entities in the page). So, to center our content, let's bind outermost {Code(nameof(LinearLayout))} paddings to an expression of a remaining room after placing a content of fixed width. Let's also bind it's width to a page width:",
            CodeBlock("""
            public override Blocks UnmanagedChildren => new()
            {
                new LinearLayout {
                    PaddingsHorizontal = new(e => (e.Root.Width - 900) / 2), 
                    Width = new (e => e.Root.Width),
                    Children = {
                        new Paragraph(Title)
                        {
                            FontSize = 24
                        },
                        Content
                    }
                }
            };
            """
            .Highlight(new CSharpHighlighter())),
            // TODO: in sample: clarify case with negative paddings
            Separator(),
            #endregion

            SectionHeader(Headers.LayoutComonentsScrollView),
            #region LayoutComonentsScrollView
            "Normally a web page content takes more than one screen. And in this case we'd expect to scroll the page while see some elements like header, side menu or some other elements pinned to a screen. Let's see how it can be done with StaticSharp.",
            $@"First of all let's add some more content to let it overflow a single screen. Let's not put too many effort in authoring it and just put just ""text text ..."" in place of text and free SVG incons in place of images (which are by the way provided by StaticSharp). So let's paste the following code to the bottom of Root page content couple of times:",
            CodeBlock("""
            new LinearLayout {
                Vertical = false,
                SecondaryGravity = 0,
                Children = {
                    new SvgIconBlock(SvgIcons.MaterialDesignIcons.Web){
                        Height = 300,
                    },
                    string.Concat(Enumerable.Repeat("text ", 50))
                }
            },
            new LinearLayout {
                Vertical = false,
                SecondaryGravity = 0,
                Children = {
                    string.Concat(Enumerable.Repeat("text ", 50)),
                    new SvgIconBlock(SvgIcons.MaterialDesignIcons.Web) {
                        Height = 300
                    },
                }
            }
            """
            .Highlight(new CSharpHighlighter())),

            $@"Now overflowing content is just clipped from the bottom side, no scroll bar appears. In order to add a scrollable area on the page in StaticSharp we need {Code(nameof(ScrollView))}. It is another layout component. We don't want header to be scrolled, while a scrollable area shoud consume all of the page but header. So let's go to {Code("PageBase")} and wrap everything but header in {Code(nameof(ScrollView))} and set its side correspondingly. Also let's move paddings set up to internal {Code(nameof(LinearLayout))} so that padding are inside scroll area (hense scroll bar is on the page border, not central column boder). Overal this results into the following code in {Code("BasePage")}:",
            CodeBlock("""
            public override Blocks UnmanagedChildren => new()
            {
                new LinearLayout {
                    Children = {
                        new Paragraph(Title)
                        {
                            FontSize = 24
                        },
                        new ScrollView {
                            Width = new(e=>e.Root.Width),
                            Height = new(e=>e.Root.Height - e.Y),
                            Child = new LinearLayout
                            {
                                PaddingsHorizontal = new (e => (e.Root.Width - 900) / 2), 
                                Width = new (e => e.Root.Width),
                                Children = { Content }
                            }
                        }
                    }
                }
            };
            """
            .Highlight(new CSharpHighlighter())),
            Separator(),
            #endregion

            SectionHeader(Headers.Navigation),
            #region Navigation
            $@"Now we've got scrollable content and fixed header. But header just contains not-formatted default text. In real world sites header frequently contains a menu with links. Let's do the same: let's add links to ""Home"", ""Portfolio"" and ""Contacts"" pages and also a link to yout Facebook account (just Facebook home for the sake of simulation). First of of all, let's add a couple child routes, e.g. ""Portfolio"" and ""Contacts"" (described in {Node.Root.GettingStarted}). Let's not care much about their contenst now, just write their names in their bodies to distinguish them. After you did it, you shold be able to see your pages when entering in browser ""http://localhost/contacts/en"" and ""http://localhost/portfolio/en"".",
            $@"To add a link to a page of our site we could just use an {Code("InternalLink")} property of a component: {Code(@"new Paragraph(""Home""){InternalLink = Node.Root}")}. Note that {Code("InternalLink")} has type {Code("Node")} (TODO: In fact Node here means Route!). StaticSharp generates a tree of yout site routes as a hierarchy of classes which is available from any page via {Code("Node")} propery, which references a current route. Also there is a shortcut for an internal link: it is possible just to pass a value of type {Code("Node")} into interpolated string and it will be converted into a link: {Code($@"new Paragraph($@""{{Node.Root}}"")")} (TODO: style is different: underline). In this case title of a link is set to route name, if you'd like to customize it you can use interpolated stirng with formatting: {Code($@"new Paragraph($@""{{Node.Root}}:Home"")")}.",
            $@"For external links there is also a property present in all componetns - {Code("ExternalLink")}. Since this properties exists in all components we can make a n image same way as text, for example let's add a Facebook icon with a link to Facebook: {Code(@"new SvgIconBlock(SvgIcons.SimpleIcons.Facebook) {ExternalLink = ""https://www.facebook.com/""}")}. One more useful property wirking in tandem with links properties is {Code("OpenLinksInANewTab")}, having a boolean type and quite straightforward meaning. So keeping in mind all written above, let's go to {Code("BasePage")} where header is defined and replace header paragraph with the following code:",
            CodeBlock("""
            new LinearLayout{
                Children = {
                    $@"{Node.Root:Home}",
                    $@"{Node.Root.Portfolio}",
                    $@"{Node.Root.Contacts}",
                    new SvgIconBlock(SvgIcons.SimpleIcons.Facebook){ 
                        ExternalLink = "https://www.facebook.com/",
                        OpenLinksInANewTab = true,
                        Height = 40
                    }
                },
                Vertical = false,
                ItemGrow = 0,
                GapGrow = 0,
                StartGapGrow = 1,
                EndGapGrow = 1
            },
            """
            .Highlight(new CSharpHighlighter())),
            Separator(),
            #endregion

            SectionHeader(Headers.PageMetadata),
            #region PageMetadata
            @$"Apart from content, which is controled by {Code("UnmanagedChildren")}, you could want to set up page metada. You can do it by overriding the following properties of {Code("StaticSharp.Page")}: {Code("SiteName")}, {Code("PageLanguage")}, {Code("Title")}, {Code("MainVisual")}, {Code("Description")}. From standpoint of common sense their meaning are quite straightforward. Next image illustrates which exact html fields they affect:",
            new Image("PageMetadata.png")
            #endregion
        };
    }
}
