using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Gears {
        public class AssetAsyncData : IAsset, IAssetAsyncData {
            string extension;
            string? contentHash;
            Task<byte[]> data;

            public AssetAsyncData(Task<byte[]> data, string extension, string? contentHash = null) {
                this.extension = extension;
                this.data = data;
                this.contentHash = contentHash;
            }

            public string Extension => extension;
            public string ContentHash {
                get {
                    if (contentHash != null)
                        return contentHash;
                    return GetContentHashAsync().GetAwaiter().GetResult();
                }
            }
            public byte[] Data => data.GetAwaiter().GetResult();
            public string Text => Encoding.UTF8.GetString(Data);
            public Task<byte[]> GetDataAsync() => data;

            public async Task<string> GetContentHashAsync() {
                if (contentHash == null) {
                    contentHash = Hash.CreateFromBytes(await GetDataAsync()).ToString();
                }
                return contentHash;
            }

        }
    }

}

