using StaticSharp.Gears;
using StaticSharp.VideoUtils;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using YoutubeExplode;

namespace StaticSharp {
    public record YoutubeDlVideoGenome(string PageUrl, int FormatCode, string Extension) : Genome<IAsset> {
        // TODO: Extension should not be a parameter

        private async Task SaveDataAsync(Cache.Slot slot, AssetAsyncData asset) {
            var content = await asset.GetDataAsync();
            slot.StoreContent(content);
        }


        protected async Task<byte[]> DownloadAsync()
        {
            var videoDownloader = new VideoDownloader();
            var tempFilePath = "./temp/" + Guid.NewGuid().ToString();
            await videoDownloader.DownloadAsync(PageUrl, FormatCode, tempFilePath);
            var fileBytes = File.ReadAllBytes(tempFilePath);
            File.Delete(tempFilePath);

            return fileBytes;
        }

        protected override void Create(out IAsset value, out Func<bool>? verify) {
            verify = null;

            var contentHash = Hash.CreateFromString($"{PageUrl} {FormatCode}").ToString();
            var slot = Cache.GetSlot(Key);

            if (slot.ContentExists()) {
                value = new BinaryAsset(
                    slot.LoadContent(),
                    "." + Extension,
                    contentHash
                    );
            } else {
                var result = new AssetAsyncData(DownloadAsync(), "." + Extension, contentHash);
                _ = SaveDataAsync(slot,result);
                value = result;
            }
        }
    }
}

