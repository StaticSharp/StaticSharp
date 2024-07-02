using FFMpegCore.Arguments;
using FFMpegCore.Pipes;
using FFMpegCore;
using Instances;
using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;


namespace StaticSharp {


    public interface JVideo : JAspectBlockResizableContent {
        public bool Play { get; set; }
        public double CurrentTime { get; set; }
        public bool Mute { get; set; }
        public double Volume { get; set; }
        public bool Controls { get; set; }
        public bool Loop { get; set; }
    }

    [RelatedStyle]
    [Scripts.FitImage]
    [ConstructorJs]
    public sealed partial class Video : AspectBlockResizableContent {

        protected override string TagName => "player";

        public Genome<IAsset> VideoGenome { get; init; }


        public Video(Genome<IAsset> videoGenome, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) {
            VideoGenome = videoGenome;        
        }

        public override void CreateContent(Context context, Tag tag, Group scriptBeforeConstructor, Group scriptAfterConstructor, string contentId, out double width, out double height) {

            var asset = VideoGenome.Result;
            var info = new VideoInfoGenome(VideoGenome).Result;
            var videoStream = info.Streams.First(x => x.CodecType == "video");            

            tag.Add(new Tag("video", contentId) {
                ["src"] = context.PathFromHostToCurrentPage.To(context.AddAsset(VideoGenome.Result)).ToString(),
                ["playsinline"] = "true",
                ["webkit-playsinline"] = "true",
                ["x5-video-player-type"] = "h5",
                ["muted"] = "",
                ["autoplay"] = "",
                /*Children = { 
                    new Tag("source"){
                        ["src"] = context.PathFromHostToCurrentPage.To(context.AddAsset(asset)).ToString(),
                        ["type"] = $"video/{asset.Extension.TrimStart('.')}"
                    }
                }*/
            });
            width = videoStream.Width.Value;
            height = videoStream.Height.Value;
        }



        /*public override void ModifyTagAndScript(Context context, Tag tag, Group script) {
            
            
            
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

        }*/







        /*public void GetMeta(Dictionary<string, string> meta, Context context) {

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
        }*/

        
    }
}
