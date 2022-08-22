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

    namespace Gears {
        [System.Diagnostics.DebuggerNonUserCode]
        public class VideoJs : BlockJs {
            //public float Before => throw new NotEvaluatableException();
        }



        public class VideoBindings<FinalJs> : BlockBindings<FinalJs> where FinalJs : new() {
            public VideoBindings(Dictionary<string, string> properties) : base(properties) {
            }
            //public Expression<Func<SpaceJs, float>> Before { set { AssignProperty(value); } }
        }
    }

    [RelatedScript]
    public sealed class Video : Block, IBlock {

        public new FlipperBindings<FlipperJs> Bindings => new(Properties);
        public override string TagName => "player";



        public string Identifier { get; init; }


        public Video(string identifier, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {
            Identifier = identifier;        
        }

        protected override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {

            var youtubeVideoId = YoutubeExplode.Videos.VideoId.TryParse(Identifier);
            if (youtubeVideoId != null) {

                var youtubeVideoManifest = await new YoutubeVideoManifestGenome(youtubeVideoId).CreateOrGetCached();

                var item = youtubeVideoManifest.Items.MaxBy(x => x.Width);


                var video = await new YoutubeVideoGenome(item).CreateOrGetCached();
                var url = context.AddAsset(video);

                elementTag["data-youtubeId"] = youtubeVideoId;
                elementTag["data-width"] = item.Width;
                elementTag["data-height"] = item.Height;

                var sources = new List<object>();
                foreach (var i in youtubeVideoManifest.Items) {
                    var iVideo = await new YoutubeVideoGenome(i).CreateOrGetCached();
                    var iUrl = context.AddAsset(iVideo);

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


            


            return new Tag() {
            };
        }
    }
}
