using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp {
    public interface IBlock {
        public Task<Tag> GenerateHtmlAsync(Context context);
    }

}