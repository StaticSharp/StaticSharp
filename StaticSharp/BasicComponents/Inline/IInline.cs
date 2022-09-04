using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp {
    public interface IInline {
        public Task<Tag> GenerateInlineHtmlAsync(Context context, string? id);
    }

}