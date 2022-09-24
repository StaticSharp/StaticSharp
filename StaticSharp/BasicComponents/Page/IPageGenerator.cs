using StaticSharp.Gears;
using System.Threading.Tasks;

namespace StaticSharp.BasicComponents.Page {
    public interface IPageGenerator {

        public Task<string> GeneratePageHtmlAsync(Context context);

    }
}
