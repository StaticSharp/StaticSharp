using StaticSharp.Gears;
using System.Threading.Tasks;

namespace StaticSharp {

    public interface IPlainTextProvider {
        Task<string> GetPlaneTextAsync(Context context);
    }


}