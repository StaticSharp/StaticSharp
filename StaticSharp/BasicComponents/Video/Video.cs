using NUglify.Html;
using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;
using SvgIcons;
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
        public interface Video : AspectBlock {
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
        public class VideoBindings<FinalJs> : AspectBlockBindings<FinalJs> {
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
    public sealed partial class Video : AspectBlock, IMainVisual {

        protected override string TagName => "player";

        public string Identifier { get; init; }


        public Video(string identifier, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) {
            Identifier = identifier;        
        }


        public override void ModifyTagAndScript(Context context, Tag tag, Group script) {
            base.ModifyTagAndScript(context, tag, script);

            var youtubeVideoId = YoutubeExplode.Videos.VideoId.TryParse(Identifier);
            if (youtubeVideoId != null) {

                var youtubeVideoManifest = new YoutubeVideoManifestGenome(youtubeVideoId).Result;

                var item = youtubeVideoManifest.Items.MaxBy(x => x.Width)!;

                tag["data-youtube-id"] = youtubeVideoId;
                //tag["data-width"] = item.Width;
                //tag["data-height"] = item.Height;

                var contentId = context.CreateId();

                SetNativeSize(script, tag.Id, item.Width, item.Height);
                script.Add($"{tag.Id}.content = {TagToJsValue(contentId)}");

                tag.Add(new Tag("content", contentId));


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
                }).Replace('"', '\'');

                tag["data-sources"] = json;
            }

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
