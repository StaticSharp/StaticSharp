using StaticSharp.Core;
using StaticSharp.Gears;
using StaticSharp.Html;

namespace StaticSharp
{
    public interface IInline: IPlainTextProvider {

        TagAndScript Generate(Context context);
        //Tag GenerateHtml(Context context);
    }
}