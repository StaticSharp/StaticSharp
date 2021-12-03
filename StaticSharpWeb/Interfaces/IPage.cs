using System.Threading.Tasks;

namespace StaticSharpWeb {
    public interface IPage {

        public Task<string> GenerateHtmlAsync(Context context);

    }
}
