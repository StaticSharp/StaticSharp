using MimeTypes;
using StaticSharp.Gears;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;

namespace StaticSharp {


    public record YoutubeVideoGenome(YoutubeVideoManifestItem ManifestItem) : Genome<Asset> {
        public override async Task<Asset> CreateAsync() {
            if (!File.Exists(ContentFilePath)) {
                var youtubeClient = new YoutubeClient();
                CreateCacheSubDirectory();
                await youtubeClient.Videos.Streams.DownloadAsync(ManifestItem, ContentFilePath);
            }
            string fileExtension = "." + ManifestItem.Container;
            return new Asset(
                () => File.ReadAllBytes(ContentFilePath),
                fileExtension,
                MimeTypeMap.GetMimeType(fileExtension),
                Hash.CreateFromString(ManifestItem.Url).ToString()
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

