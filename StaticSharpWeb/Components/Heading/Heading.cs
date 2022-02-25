using StaticSharpGears;
using StaticSharpGears.Public;
using StaticSharpWeb.Html;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace StaticSharpWeb {





    public class Heading : IContainerConstraints<ITextAnchorsProvider> {

        public object? Style { get; set; } = null;

        public string Text { get; set; }

        public Heading(string text) {
            Text = text;
        }

        public async Task<INode> GenerateHtmlAsync(Context context) {
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
                new JSCall(Anchors.FillTextAnchorsJsPath).Generate(context),
                new JSCall(Anchors.ReduceFontSizeOnOverflowJsPath).Generate(context),
                new TextNode(Text)
            };                
        }
    }

    public class SectionHeading :  IElement, IContainerConstraints<ITextAnchorsProvider> {
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

        /*public async Task<INode> GenerateInlineHtmlAsync(Context context) => string.IsNullOrWhiteSpace(Identifier)
            ? new Tag("h2", new { Class = "Heading" }) { Text }
            : new Tag("h2", new { id = Identifier }){
                new Tag("a", new { href = "#" + Identifier, title = "Heading anchor" })
            };*/

        public async Task<INode> GenerateHtmlAsync(Context context) {

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
    }
}
