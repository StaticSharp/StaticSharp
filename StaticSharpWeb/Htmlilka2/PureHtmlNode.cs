using System.IO;
using System.Text;

namespace StaticSharp.Html {

    public class PureHtmlNode : INode {
        public string Content { get; set; }

        public PureHtmlNode(string text) => Content = text;

        public void WriteHtml(StringBuilder builder) => 
            builder.Append(Content);

    }
}