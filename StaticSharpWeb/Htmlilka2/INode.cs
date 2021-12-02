using System.Text;

namespace CsmlWeb.Html {

    public interface INode {
        public void WriteHtml(StringBuilder builder);
    }


    public static class INodeStatic {
        public static string GetHtml(this INode _this) {
            var result = new StringBuilder();
            _this.WriteHtml(result);
            return result.ToString();
        }
    }
}