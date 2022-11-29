using StaticSharp.Gears;
using System.IO;
using System.Threading.Tasks;

namespace StaticSharp {
    public record YoutubeVideoGenome(YoutubeVideoManifestItem ManifestItem) : Genome<IAsset> {

        private async Task SaveDataAsync(YoutubeVideoAsset asset) {
            var content = await asset.GetBytesAsync();
            CreateCacheSubDirectory();
            await FileUtils.WriteAllBytesAsync(ContentFilePath, content);
        }


        public override IAsset Create() {

            if (!File.Exists(ContentFilePath)) {                
                var result = new YoutubeVideoAsset(ManifestItem);
                _ = SaveDataAsync(result);
                return result;
            }

            return new RestoredAsset(
                YoutubeVideoAsset.GetFileExtension(ManifestItem),
                YoutubeVideoAsset.GetMediaType(ManifestItem),
                YoutubeVideoAsset.GetContentHash(ManifestItem),
                File.ReadAllBytes(ContentFilePath)
                );

        }
    }


    /*namespace Gears {
        public class YoutubeVideoAsset : CacheableToFile<YoutubeVideoGenome> , Asset {

            public YoutubeVideoAsset

            public string MediaType => MimeTypeMap.GetMimeType(FileExtension);

            public string ContentHash => Hash.CreateFromString(Genome.ManifestItem.Url).ToString();

            public string FileExtension => "."+Genome.ManifestItem.Container;

            protected override async Task CreateAsync() {
                if (!LoadData<object>(out var data)) {
                    var youtubeClient = new YoutubeClient();
                    CreateCacheSubDirectory();
                    await youtubeClient.Videos.Streams.DownloadAsync(Genome.ManifestItem,ContentFilePath);                    
                    StoreData(data);
                }
            }
        }
    }*/



}

