using StaticSharp.Gears;
using System.Threading.Tasks;

namespace StaticSharp.Gears {
    public interface IPageGenerator {
        public string GeneratePageHtml(Context context);

    }
}
