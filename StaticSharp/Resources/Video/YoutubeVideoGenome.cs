using StaticSharp.Gears;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using YoutubeExplode;

namespace StaticSharp {
    public record YoutubeVideoGenome(YoutubeVideoManifestItem ManifestItem) : Genome<IAsset> {

        private async Task SaveDataAsync(Cache.Slot slot, AssetAsyncData asset) {
            var content = await asset.GetDataAsync();
            slot.StoreContent(content);
        }


        public async Task<byte[]> DownloadAsync(YoutubeVideoManifestItem manifestItem) {
            var youtubeClient = new YoutubeClient();
            var stream = await youtubeClient.Videos.Streams.GetAsync(manifestItem);
            using (MemoryStream memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        protected override void Create(out IAsset value, out Func<bool>? verify) {
            verify = null;

            var contentHash = Hash.CreateFromString(ManifestItem.Url).ToString();
            var extension = "." + ManifestItem.Container;

            var slot = Cache.GetSlot(Key);

            if (slot.ContentExists()) {
                value = new BinaryAsset(
                    slot.LoadContent(),
                    extension,
                    contentHash
                    );
            } else { 
                var result = new AssetAsyncData(DownloadAsync(ManifestItem), extension, contentHash);
                _ = SaveDataAsync(slot,result);
                value = result;
            }

            

        }
    }
}

