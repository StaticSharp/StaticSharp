using StaticSharp.Gears;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace StaticSharp {



    public record YoutubeVideoManifestGenome(string VideoId) : Genome<YoutubeVideoManifestGenome, YoutubeVideoManifestResource>{
    }



    namespace Gears {

        public class YoutubeVideoManifestResourceData {
            public List<YoutubeVideoManifestItem> Items = new();
        }
        public class YoutubeVideoManifestItem : IStreamInfo, Gears.IKeyProvider {
            public string Key => Gears.KeyUtils.Combine<YoutubeVideoManifestItem>(Url);

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


        public class YoutubeVideoManifestResource : Cacheable<YoutubeVideoManifestGenome, YoutubeVideoManifestResourceData> {

            public override string? CharSet => null;

            public IEnumerable<YoutubeVideoManifestItem> Items => CachedData.Items;
            protected override async Task CreateAsync() {
                var validContainers = new string[] { "mp4" };
                if (!LoadData()) {
                    CachedData = new();

                    var youtubeClient = new YoutubeClient();
                    var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(Genome.VideoId);

                    foreach (var i in streamManifest.GetMuxedStreams().Where(x=> validContainers.Contains(x.Container.Name))) {
                        var item = new YoutubeVideoManifestItem();
                        item.Width = i.VideoResolution.Width;
                        item.Height = i.VideoResolution.Height;
                        item.Url = i.Url;
                        item.Container = i.Container.Name;
                        item.Size = i.Size.Bytes;
                        item.Bitrate = i.Bitrate.BitsPerSecond;

                        CachedData.Items.Add(item);
                    }

                    CreateCacheSubDirectory();
                    StoreData();
                }
            }
        }
    }



}

