using System.Threading.Tasks;

namespace StaticSharp {
    public interface IMutableAsset {
        Task<bool> GetValidAsync();
        public void DeleteCacheSubDirectory();
    }
}

