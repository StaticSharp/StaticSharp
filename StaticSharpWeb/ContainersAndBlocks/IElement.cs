using System.Threading.Tasks;

namespace StaticSharpWeb;


public interface IElement {
    Task<Html.INode> GenerateHtmlAsync(Context context);
}
