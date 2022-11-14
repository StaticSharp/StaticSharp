using Scopes.C;
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp {
    public interface IBlock {
        public Task<Tag> GenerateHtmlAsync(Context context, Role? role);

        //public Task<Scopes.Node> GenerateConstructor(Context context, string? id);


    }

}