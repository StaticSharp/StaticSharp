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



            CacheableMaterialDesignIcon cacheableMaterialDesignIcon = new CacheableMaterialDesignIcon.Constructor(MaterialDesignIcons.Link).CreateOrGetCached();

            //await cacheableMaterialDesignIcon.Job;
            var svgCode = await cacheableMaterialDesignIcon.Code;

            /*XmlDocument doc = new XmlDocument();
            doc.LoadXml(svgCode);
            doc.DocumentElement.ChildNodes.*/

            XDocument svg = XDocument.Parse(svgCode);
            var svgElement = svg.Elements().FirstOrDefault(x => x.Name.LocalName == "svg");
            svgElement.SetAttributeValue("fill", "#ff8000");


            var finalCode = svgElement.ToString();

            foreach (var e in svg.Elements()) { 
                Console.WriteLine(e.Name);
            }


            var dataSvg = Uri.EscapeDataString(await cacheableMaterialDesignIcon.Code);
            //context.Includes.Require()

            //var a = new Tag("a" new { href =  });

            return new Tag("h2") {
                new Tag("div", new{ Class = "HeadingLink"}),
                new TextNode(Caption),
                new JSCall(new AbsolutePath($"{GetType().Name}.js")).Generate(context)
            };
            /*=> string.IsNullOrWhiteSpace(Identifier)
            ? new Tag("h2", new { Class = "Heading" }) { Caption }
            : new Tag("h2", new { id = Identifier }){
                new Tag("a", new { href = "#" + Identifier, title = "Heading anchor" })
            };*/

        }
    }
}
