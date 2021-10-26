using System.Threading.Tasks;

namespace CsmlWeb {
    public interface IPage {

        public Task<string> GenerateHtmlAsync(Context context);

    }
}
