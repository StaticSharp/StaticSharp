using System.Threading.Tasks;

namespace StaticSharp {
    public interface IMutableAsset {
        bool GetValid();
        public void DeleteCacheSubDirectory();
    }
}

