using System.Threading.Tasks;

namespace StaticSharp.Gears {
    public interface IPage {

        public Task<string> GeneratePageHtmlAsync(Context context);

    }
}
