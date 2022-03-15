
using StaticSharpWeb.Html;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public abstract class Component : IElement {
        public abstract Task<Tag> GenerateHtmlAsync(Context context);
    }
}
