using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface IPlainTextProvider {

        Task<string> GetPlaneTextAsync(Context context);
    }


}