using System.Runtime.CompilerServices;

namespace StaticSharp.Alpha {
    public static partial class Static {

        public static Paragraph HX(string text,
            float fontSize,
            FontWeight fontWeight,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) {

            return new Paragraph(text, callerFilePath, callerLineNumber) {
                FontSize = fontSize,
                FontStyle = new FontStyle(fontWeight),
            };
        }
        public static Paragraph H1(
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            return HX(text,96,FontWeight.ExtraLight, callerFilePath,callerLineNumber);
        }
        public static Paragraph H2(
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            return HX(text, 60, FontWeight.Light, callerFilePath, callerLineNumber);
        }
        public static Paragraph H3(
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            return HX(text, 48, FontWeight.Regular, callerFilePath, callerLineNumber);
        }
        public static Paragraph H4(
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            return HX(text, 34, FontWeight.Medium, callerFilePath, callerLineNumber);
        }
        public static Paragraph H5(
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            return HX(text, 24, FontWeight.SemiBold, callerFilePath, callerLineNumber);
        }
        public static Paragraph H6(
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            return HX(text, 20, FontWeight.Bold, callerFilePath, callerLineNumber);
        }


        /*public static IBlock H1(string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            var paragraph = new Paragraph(callerFilePath, callerLineNumber);
            paragraph.AppendLiteral(text, callerFilePath, callerLineNumber);
            return H1(paragraph);
        }*/

    }


    
    /*public class Heading : Block<Symbolic.Block> {

        public record HeadingStyle(
            FontFamily FontFamily,
            FontStyle FontStyle,
            float FontSize
            ) { }

        public string Text { get; set; }

        public Modifier Modifier { get; private set; }
        public Paragraph Paragraph { get; private set; }

        public Heading(
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {
            Paragraph = new Paragraph();

            Text = text;
        }


        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }

        public override async Task<Tag> GenerateHtmlAsync(Context context) {
            AddRequiredInclues(context.Includes);
            return new Tag("h") {
                CreateScriptBefore(),
                await Task.WhenAll(children.Select(x=>x.GenerateInlineHtmlAsync(context))),
                CreateScriptAfter()
            };
        }



        public async Task<Tag> GenerateHtmlAsync(Context context) {
            return new Tag("h2", 
                new {
                    style = SoftObject.MergeObjects(
                        new {
                            MarginTop = $"{context.Theme.HeadingSpacing}px",
                        },
                        Style
                        )
                })
            {
                //new JSCall(Layout.TextJsPath,null,"Before").Generate(context),
                //new JSCall(Layout.ReduceFontSizeOnOverflowJsPath).Generate(context),
                new TextNode(Text)
            };                
        }
    }*/

    /*public class SectionHeading :  IElement, IContainerConstraints<ITextAnchorsProvider> {
        public string Text { get; set; }

        private string _identifier;
        public string Identifier { 
            get {
                if (string.IsNullOrEmpty(_identifier))
                    return Text.Replace(" ", "_");
                return _identifier;
            }
            set { _identifier = value; }
        }

        public SectionHeading(string text, string identifier = null) =>
            (Text, Identifier) = (text, identifier);

        *//*public async Task<INode> GenerateInlineHtmlAsync(Context context) => string.IsNullOrWhiteSpace(Identifier)
            ? new Tag("h2", new { Class = "Heading" }) { Text }
            : new Tag("h2", new { id = Identifier }){
                new Tag("a", new { href = "#" + Identifier, title = "Heading anchor" })
            };*//*

        public async Task<Tag> GenerateHtmlAsync(Context context) {

            var anchorIconSource = new CacheableHttpRequest.Constructor(
                MaterialDesignIcons.VectorLink.GetSvgUri()
                )
                .CreateOrGetCached();

            var svg = new Svg(await anchorIconSource.ReadAllTextAsync());
            svg.FillColor = context.Theme.HeadingAnchorIconColor;
            var svgDataUri = svg.DataUri;


            return new Tag("h2", new { id = Identifier, style = "display: flex;" }) {
                new Tag("span", new{ style = "margin-right: auto;"}) { 
                    new TextNode(Text)
                }
                ,
                new Tag("a", new{                    
                    href = "#" + Identifier,
                    title = "Heading anchor",
                    style = "display: contents"
                    }){ 
                    new Tag("img", new{
                        style = "display: inline;",
                        src = svgDataUri
                    })
                },

                new JSCall(AbsolutePath($"{GetType().Name}.js")).Generate(context)
            };
        }
    }*/
}
