using StaticSharp.Core;
using StaticSharp.Gears;
using StaticSharp.Html;

namespace StaticSharp
{
    public interface IInline: IPlainTextProvider {
        public Tag GenerateHtml(Context context);
    }
}