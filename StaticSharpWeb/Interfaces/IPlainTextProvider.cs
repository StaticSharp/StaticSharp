using System.Threading.Tasks;

namespace CsmlWeb {

    public interface IPlainTextProvider {

        Task<string> GetPlaneTextAsync(Context context);
    }


}