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
        public class Video : Block {
            public bool Play => NotEvaluatableValue<bool>();
            public bool PlayActual => NotEvaluatableValue<bool>();

            public double Position => NotEvaluatableValue<double>();
            public double PositionActual => NotEvaluatableValue<double>();

            public bool Mute => NotEvaluatableValue<bool>();
            public bool MuteActual => NotEvaluatableValue<bool>();

            public double Volume => NotEvaluatableValue<double>();
            public double VolumeActual => NotEvaluatableValue<double>();


            public bool PreferPlatformPlayer => NotEvaluatableValue<bool>();
            public bool Controls => NotEvaluatableValue<bool>();
            public bool Loop => NotEvaluatableValue<bool>();
        }
    }


    namespace Gears {
        public class VideoBindings<FinalJs> : BlockBindings<FinalJs> where FinalJs : new() {
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


        public Video(string identifier, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {
            Identifier = identifier;        
        }

        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {
            

            var youtubeVideoId = YoutubeExplode.Videos.VideoId.TryParse(Identifier);
            if (youtubeVideoId != null) {

                var youtubeVideoManifest = await new YoutubeVideoManifestGenome(youtubeVideoId).CreateOrGetCached();

                var item = youtubeVideoManifest.Items.MaxBy(x => x.Width)!;

                elementTag["data-youtube-id"] = youtubeVideoId;
                elementTag["data-width"] = item.Width;
                elementTag["data-height"] = item.Height;

                var sources = new List<object>();
                foreach (var i in youtubeVideoManifest.Items) {
                    var iVideo = await new YoutubeVideoGenome(i).CreateOrGetCached();
                    var iUrl = context.PathFromHostToCurrentPage.To(await context.AddAssetAsync(iVideo)).ToString();

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

            await base.ModifyHtmlAsync(context, elementTag);
        }



        public async Task GetMetaAsync(Dictionary<string, string> meta, Context context) {

            var youtubeVideoId = YoutubeExplode.Videos.VideoId.TryParse(Identifier);
            if (youtubeVideoId != null) {
                var youtubeVideoManifest = await new YoutubeVideoManifestGenome(youtubeVideoId).CreateOrGetCached();
                var item = youtubeVideoManifest.Items.MaxBy(x => x.Width)!;
                var video = await new YoutubeVideoGenome(item).CreateOrGetCached();
                var url = await context.AddAssetAsync(video);

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
