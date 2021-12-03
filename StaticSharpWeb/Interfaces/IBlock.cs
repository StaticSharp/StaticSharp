using System.Threading.Tasks;

namespace StaticSharpWeb {
    public interface IBlock {
        Task<Html.INode> GenerateBlockHtmlAsync(Context context);
    }


    /*public static class IBlockStatic {
    public static async Task<Html.INode> SafeGenerateBlockHtmlAsync(this IBlock _this,
        object parent,
        IContentPathResolver contentPathResolver,
        IPageStorage pageStorage) {

        if (_this == null) return null;
        return await _this.GenerateBlockHtmlAsync(parent, contentPathResolver, pageStorage);
    }
}*/

}