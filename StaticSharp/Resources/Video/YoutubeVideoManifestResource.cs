using System.Collections.Generic;
using YoutubeExplode.Videos.Streams;

namespace StaticSharp {


    namespace Gears {
        public class YoutubeVideoManifestResource {
            public List<YoutubeVideoManifestItem> Items { get; set; } = new();
        }
        public class YoutubeVideoManifestItem : IStreamInfo, IKeyProvider {
            public string Key => KeyUtils.Combine<YoutubeVideoManifestItem>(Url);

            public string Url = null!;
            public string Container = null!;
            public long Size;
            public long Bitrate;

            public int Width;
            public int Height;
            string IStreamInfo.Url => Url;
            Container IStreamInfo.Container => new Container(Container);
            FileSize IStreamInfo.Size => new FileSize(Size);
            Bitrate IStreamInfo.Bitrate => new Bitrate(Bitrate);
        }
        
    }



}

