using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp {
    public interface IInline: IPlainTextProvider {
        public Task<Tag> GenerateHtmlAsync(Context context);
    }

}