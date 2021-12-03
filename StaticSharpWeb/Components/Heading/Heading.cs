using StaticSharpWeb.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public class Heading : IInline, IBlock {
        public string Caption { get; set; }
        public string UserDefinedIdentifier { get; set; }
        public string Id => UserDefinedIdentifier ?? Caption.Replace(" ", "_");

        public Heading(string caption, string identifier) =>
            (Caption, UserDefinedIdentifier) = (caption, identifier);




        public async Task<INode> GenerateInlineHtmlAsync(Context context) => string.IsNullOrWhiteSpace(Id)
            ? new Tag("h2", new { Class = "Heading" }) { Caption }
            : new Tag("h2", new { id = Id }){
                new Tag("a", new { href = "#" + Id, title = "Heading anchor" })
            };
        //x.Attribute("id", Id)
        //        x.AddTag("a", a => {
        //            a.AddClasses("Link");
        //            a.Attribute("href", "#" + Id);
        //            a.Attribute("title", "Heading anchor");
        //        })

        public async Task<INode> GenerateBlockHtmlAsync(Context context) => string.IsNullOrWhiteSpace(Id)
            ? new Tag("h2", new { Class = "Heading" }) { Caption }
            : new Tag("h2", new { id = Id }){
                new Tag("a", new { href = "#" + Id, title = "Heading anchor" })
            };
    }
}
