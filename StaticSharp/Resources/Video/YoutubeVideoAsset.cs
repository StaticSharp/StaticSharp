using MimeTypes;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;

namespace StaticSharp {

    namespace Gears {

        public class YoutubeVideoAsset : IAsset {
            YoutubeVideoManifestItem manifestItem;
            byte[]? data;
            public YoutubeVideoAsset(YoutubeVideoManifestItem manifestItem) {
                this.manifestItem = manifestItem;
            }

            public async Task<byte[]> GetBytesAsync() {
                if (data == null) {
                    var youtubeClient = new YoutubeClient();
                    var stream = await youtubeClient.Videos.Streams.GetAsync(manifestItem);
                    using (MemoryStream memoryStream = new MemoryStream()) {
                        stream.CopyTo(memoryStream);
                        data = memoryStream.ToArray();
                    }
                }
                return data;
            }

            public Task<string> GetContentHashAsync() => Task.FromResult(GetContentHash(manifestItem));

            public static string GetContentHash(YoutubeVideoManifestItem manifestItem) => Hash.CreateFromString(manifestItem.Url).ToString();

            public Task<string> GetFileExtensionAsync() => Task.FromResult(GetFileExtension(manifestItem));

            public static string GetFileExtension(YoutubeVideoManifestItem manifestItem) => "." + manifestItem.Container;

            public Task<string> GetMediaTypeAsync() => Task.FromResult(GetMediaType(manifestItem));

            public static string GetMediaType(YoutubeVideoManifestItem manifestItem) => MimeTypeMap.GetMimeType(GetFileExtension(manifestItem));

            public Task<string> GetTextAsync() {
                throw new System.NotImplementedException();
            }




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

