using MimeTypes;
using StaticSharp.Gears;
using System.Threading.Tasks;
using YoutubeExplode;

namespace StaticSharp {


    public record YoutubeVideoGenome(YoutubeVideoManifestItem ManifestItem) : AssetGenome<YoutubeVideoGenome, YoutubeVideoAsset> {
    }


    namespace Gears {
        public class YoutubeVideoAsset : Cacheable<YoutubeVideoGenome, object> , IAsset {
            public string MediaType => MimeTypeMap.GetMimeType(FileExtension);

            public string ContentHash => Hash.CreateFromString(Genome.ManifestItem.Url).ToString();

            public string FileExtension => "."+Genome.ManifestItem.Container;

            protected override async Task CreateAsync() {
                if (!LoadData()) {
                    CachedData = new();
                    var youtubeClient = new YoutubeClient();
                    CreateCacheSubDirectory();
                    await youtubeClient.Videos.Streams.DownloadAsync(Genome.ManifestItem,ContentFilePath);                    
                    StoreData();
                }
            }
        }
    }



}

