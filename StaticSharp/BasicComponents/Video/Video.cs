using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YoutubeExplode;

namespace StaticSharp {


    namespace Js {
        public interface Video : Block {
            public double Aspect  { get; }
            public bool Play { get; }
            public bool PlayActual { get; }

            public double Position  { get; }
            public double PositionActual  { get; }

            public bool Mute { get; }
            public bool MuteActual { get; }

            public double Volume  { get; }
            public double VolumeActual  { get; }


            public bool PreferPlatformPlayer { get; }
            public bool Controls { get; }
            public bool Loop { get; }
        }
    }


    namespace Gears {
        public class VideoBindings<FinalJs> : BlockBindings<FinalJs> {
            public Binding<bool> Play { set { Apply(value); } }
            public Binding<double> Position { set { Apply(value); } }
            public Binding<bool> Mute { set { Apply(value); } }
            public Binding<double> Volume { set { Apply(value); } }


            public Binding<bool> PreferPlatformPlayer { set { Apply(value); } }
            public Binding<bool> Controls { set { Apply(value); } }            
            public Binding<bool> Loop { set { Apply(value); } }

        }
    }



    [Mix(typeof(VideoBindings<Js.Video>))]
    [ConstructorJs]
    public sealed partial class Video : Block, IBlock , IMainVisual {

        protected override string TagName => "player";

        public string Identifier { get; init; }


        public Video(string identifier, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) {
            Identifier = identifier;        
        }

        protected override void ModifyHtml(Context context, Tag elementTag) {
            

            var youtubeVideoId = YoutubeExplode.Videos.VideoId.TryParse(Identifier);
            if (youtubeVideoId != null) {

                var youtubeVideoManifest = new YoutubeVideoManifestGenome(youtubeVideoId).Result;

                var item = youtubeVideoManifest.Items.MaxBy(x => x.Width)!;

                elementTag["data-youtube-id"] = youtubeVideoId;
                elementTag["data-width"] = item.Width;
                elementTag["data-height"] = item.Height;

                var sources = new List<object>();
                foreach (var i in youtubeVideoManifest.Items) {
                    var iVideo = new YoutubeVideoGenome(i).Result;
                    var iUrl = context.PathFromHostToCurrentPage.To(context.AddAsset(iVideo)).ToString();

                    sources.Add(new {
                        size = new {
                            x = i.Width,
                            y = i.Height
                        },
                        url = iUrl
                    });
                }

                string json = JsonSerializer.Serialize(sources, new JsonSerializerOptions() {
                    IncludeFields = true                    
                }).Replace('"','\'');

                elementTag["data-sources"] = json;
            }

            base.ModifyHtml(context, elementTag);
        }



        public void GetMeta(Dictionary<string, string> meta, Context context) {

            var youtubeVideoId = YoutubeExplode.Videos.VideoId.TryParse(Identifier);
            if (youtubeVideoId != null) {
                var youtubeVideoManifest = new YoutubeVideoManifestGenome(youtubeVideoId).Result;
                var item = youtubeVideoManifest.Items.MaxBy(x => x.Width)!;
                var video = new YoutubeVideoGenome(item).Result;
                var url = context.AddAsset(video);

                meta["og:type"] = "video";
                meta["og:video"] = (context.BaseUrl + url).ToString();
                meta["og:video:width"] = item.Width.ToString();
                meta["og:video:height"] = item.Height.ToString();

            } else {
                throw new NotImplementedException();
            }
        }
    }
}
