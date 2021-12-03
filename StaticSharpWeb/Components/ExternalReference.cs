using StaticSharpWeb.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public class ExternalReference : IInline {
        private string Href { get; set; }
        private string Text { get; set; }
        private Image Image { get; set; }
        private string Tooltip { get; set; }

        private ExternalReference(string href, string tooltip = "")
            => (Href, Tooltip) = (href, tooltip);        

        public ExternalReference(string href, string text = "", string tooltip = "") : this(href, tooltip) 
            => Text = text;


        public ExternalReference(string href, Image image, string tooltip = "") : this(href, tooltip)
            => Image = image;


        public async Task<INode> GenerateInlineHtmlAsync(Context context) {
            var result = new Tag("a", new { href = Href, Class = "\"Text\", \"Reference\", \"ExternalReference\"" });
            if (Image != null) {
                result.Attributes.Add("style", "display: block");
                result.Add(await Image.GenerateBlockHtmlAsync(context));
            } else if (string.IsNullOrEmpty(Text)) {
                result.Add(Href);
            } else {
                result.Add(Text);
            }
            if (!string.IsNullOrEmpty(Tooltip)) {
                result.Attributes.Add("title", Tooltip);
            } else {
                if (string.IsNullOrEmpty(Tooltip) && !string.IsNullOrEmpty(Text)) {
                    result.Attributes.Add("title", Href);
                }
            }
            return result;
        }
    }
}
