using System.Threading.Tasks;

namespace CsmlWeb {
    public interface IInline {
        Task<Html.INode> GenerateInlineHtmlAsync(Context context);
    }


}