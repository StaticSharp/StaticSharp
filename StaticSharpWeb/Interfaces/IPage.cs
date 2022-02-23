using System.Threading.Tasks;

namespace StaticSharpWeb {
    public interface IPage {

        public Task<string> GeneratePageHtmlAsync(Context context);

    }
}
