using StaticSharpGears;
using StaticSharpGears.Public;
using StaticSharpWeb.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace StaticSharpWeb {

    

    public class Heading : IInline, IBlock, IContainerConstraints<ITextAnchorsProvider> {
        public string Caption { get; set; }

        private string _identifier;
        public string Identifier { 
            get {
                if (string.IsNullOrEmpty(_identifier))
                    return Caption.Replace(" ", "_");
                return _identifier;
            }
            set { _identifier = value; }
        }

        public Heading(string caption, string identifier = null) =>
            (Caption, Identifier) = (caption, identifier);




        public async Task<INode> GenerateInlineHtmlAsync(Context context) => string.IsNullOrWhiteSpace(Identifier)
            ? new Tag("h2", new { Class = "Heading" }) { Caption }
            : new Tag("h2", new { id = Identifier }){
                new Tag("a", new { href = "#" + Identifier, title = "Heading anchor" })
            };
        //x.Attribute("id", Id)
        //        x.AddTag("a", a => {
        //            a.AddClasses("Link");
        //            a.Attribute("href", "#" + Id);
        //            a.Attribute("title", "Heading anchor");
        //        })

        public async Task<INode> GenerateBlockHtmlAsync(Context context) {

            var anchorIconSource = new CacheableHttpRequest.Constructor(
                MaterialDesignIcons.VectorLink.GetSvgUri()
                )
                .CreateOrGetCached();

            var svgCode = await anchorIconSource.ReadAllTextAsync();

            var svg = new Svg(await anchorIconSource.ReadAllTextAsync());
            svg.FillColor = context.Theme.HeadingAnchorIconColor;
            var svgDataUri = svg.DataUri;


            return new Tag("h2", new { id = Identifier, style = "display: flex;" }) {
                new Tag("span", new{ style = "margin-right: auto;"}) { 
                    new TextNode(Caption)
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
            /*=> string.IsNullOrWhiteSpace(Identifier)
            ? new Tag("h2", new { Class = "Heading" }) { Caption }
            : new Tag("h2", new { id = Identifier }){
                new Tag("a", new { href = "#" + Identifier, title = "Heading anchor" })
            };*/

        }
    }
}
