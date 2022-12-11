using StaticSharp.Gears;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using YoutubeExplode;

namespace StaticSharp {
    public record YoutubeVideoGenome(YoutubeVideoManifestItem ManifestItem) : Genome<IAsset> {

        private async Task SaveDataAsync(AssetAsyncData asset) {
            var content = await asset.GetDataAsync();
            CreateCacheSubDirectory();
            await FileUtils.WriteAllBytesAsync(ContentFilePath, content);
        }


        public async Task<byte[]> DownloadAsync(YoutubeVideoManifestItem manifestItem) {
            var youtubeClient = new YoutubeClient();
            var stream = await youtubeClient.Videos.Streams.GetAsync(manifestItem);
            using (MemoryStream memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public override IAsset Create() {
            var contentHash = Hash.CreateFromString(ManifestItem.Url).ToString();
            var extension = "." + ManifestItem.Container;

            if (!File.Exists(ContentFilePath)) {
                
                var result = new AssetAsyncData(DownloadAsync(ManifestItem), extension, contentHash);
                _ = SaveDataAsync(result);
                return result;
            }

            return new BinaryAsset(
                File.ReadAllBytes(ContentFilePath),
                extension,
                contentHash
                );

        }
    }
}

