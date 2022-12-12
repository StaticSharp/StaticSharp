using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp {
    public interface IInline: IPlainTextProvider {
        public Tag GenerateHtml(Context context, Role? role);
    }

}