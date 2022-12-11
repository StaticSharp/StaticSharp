using StaticSharp.Gears;
using System.Threading.Tasks;

namespace StaticSharp {
    public interface IMutableAsset: IAsset {
        bool GetValid();
        //public void DeleteCacheSubDirectory();
    }
}

