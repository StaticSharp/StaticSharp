using System.Threading.Tasks;

namespace StaticSharp {

    namespace Gears {
        public class AsyncAsset : IAsset {

            public Task<IAsset> assetTask;

            public AsyncAsset(Task<IAsset> assetTask) {
                this.assetTask = assetTask;
            }

            public async Task<byte[]> GetBytesAsync() {
                return await (await assetTask).GetBytesAsync();
            }

            public async Task<string> GetContentHashAsync() {
                return await (await assetTask).GetContentHashAsync();
            }

            public async Task<string> GetFileExtensionAsync() {
                return await (await assetTask).GetFileExtensionAsync();
            }

            public async Task<string> GetMediaTypeAsync() {
                return await (await assetTask).GetMediaTypeAsync();
            }

            public async Task<string> GetTextAsync() {
                return await (await assetTask).GetTextAsync();
            }
        }
    }

}

