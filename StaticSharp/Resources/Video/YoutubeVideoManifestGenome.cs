using StaticSharp.Gears;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;

namespace StaticSharp {
    public record YoutubeVideoManifestGenome(string VideoId) : Genome<Task<YoutubeVideoManifestResource>>{


        private async Task<YoutubeVideoManifestResource> CreateAndStore() {

            var validContainers = new string[] { "mp4" };

            var youtubeClient = new YoutubeClient();
            var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(VideoId);
            var result = new YoutubeVideoManifestResource();
            foreach (var i in streamManifest.GetMuxedStreams().Where(x => validContainers.Contains(x.Container.Name))) {
                var item = new YoutubeVideoManifestItem();
                item.Width = i.VideoResolution.Width;
                item.Height = i.VideoResolution.Height;
                item.Url = i.Url;
                item.Container = i.Container.Name;
                item.Size = i.Size.Bytes;
                item.Bitrate = i.Bitrate.BitsPerSecond;

                result.Items.Add(item);
            }

            CreateCacheSubDirectory();
            StoreData(result);
            return result;
        }

        public override Task<YoutubeVideoManifestResource> Create() {            
            if (!LoadData<YoutubeVideoManifestResource>(out var data)) {
                return CreateAndStore();
            }
            return Task.FromResult(data);
        }
    }



}

