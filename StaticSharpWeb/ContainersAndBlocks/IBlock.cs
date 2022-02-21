using System.Threading.Tasks;

namespace StaticSharpWeb;
public interface IBlock {
    Task<Html.INode> GenerateBlockHtmlAsync(Context context);
}
