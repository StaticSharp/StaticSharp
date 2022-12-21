using StaticSharp.Gears;
using System;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;

namespace StaticSharp {
    public record YoutubeVideoManifestGenome(string VideoId) : Genome<YoutubeVideoManifestResource>{


        private YoutubeVideoManifestResource CreateAndStore(Cache.Slot slot) {

            var validContainers = new string[] { "mp4" };

            var youtubeClient = new YoutubeClient();
            var streamManifest = youtubeClient.Videos.Streams.GetManifestAsync(VideoId).GetAwaiter().GetResult();
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
            slot.StoreData(result);
            return result;
        }

        protected override void Create(out YoutubeVideoManifestResource value, out Func<bool>? verify) {
            verify = null;
            var slot = Cache.GetSlot(Key);
            if (!slot.LoadData(out value)) {
                value = CreateAndStore(slot);
            }
        }
    }



}

