using StaticSharp.Gears;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;

namespace StaticSharp {
    public record YoutubeVideoManifestGenome(string VideoId) : Genome<YoutubeVideoManifestResource>{
        public override async Task<YoutubeVideoManifestResource> CreateAsync() {

            var validContainers = new string[] { "mp4" };
            if (!LoadData<YoutubeVideoManifestResource>(out var data)) {

                var youtubeClient = new YoutubeClient();
                var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(VideoId);

                foreach (var i in streamManifest.GetMuxedStreams().Where(x => validContainers.Contains(x.Container.Name))) {
                    var item = new YoutubeVideoManifestItem();
                    item.Width = i.VideoResolution.Width;
                    item.Height = i.VideoResolution.Height;
                    item.Url = i.Url;
                    item.Container = i.Container.Name;
                    item.Size = i.Size.Bytes;
                    item.Bitrate = i.Bitrate.BitsPerSecond;

                    data.Items.Add(item);
                }

                CreateCacheSubDirectory();
                StoreData(data);
            }
            return data;
        }
    }



}

