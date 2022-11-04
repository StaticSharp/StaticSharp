using StaticSharp.Gears;
using System.Threading.Tasks;

namespace StaticSharp.Gears {
    public interface IPageGenerator {

        public Task<string> GeneratePageHtmlAsync(Context context);

    }
}
