using StaticSharp.Gears;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace StaticSharp {



    public record YoutubeVideoManifestGenome(string VideoId) : Genome<YoutubeVideoManifestGenome, YoutubeVideoManifestResource>{
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



    namespace Gears {

        public class YoutubeVideoManifestItem : IStreamInfo, IKeyProvider {
            public string Key => KeyUtils.Combine<YoutubeVideoManifestItem>(Url);

            public string Url;
            public string Container;
            public long Size;
            public long Bitrate;

            public int Width;
            public int Height;


            string IStreamInfo.Url => Url;
            Container IStreamInfo.Container => new Container(Container);
            FileSize IStreamInfo.Size => new FileSize(Size);
            Bitrate IStreamInfo.Bitrate => new Bitrate(Bitrate);
        }


        public class YoutubeVideoManifestResource : CacheableToFile<YoutubeVideoManifestGenome> {

            /*class Data {
                public List<YoutubeVideoManifestItem> Items = new();
            }*/

            public override string? CharSet => null;

            //Data data = null!;
            public List<YoutubeVideoManifestItem> Items => new();
            protected override async Task CreateAsync() {
                
            }
        }
    }



}

