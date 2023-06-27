using ImageMagick;
using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;


namespace StaticSharp {


    public interface JImage : JAspectBlockResizableContent {
        //public double Aspect  { get; }
    }

    [Scripts.FitImage]
    [ConstructorJs]
    public partial class Image : AspectBlockResizableContent, IMainVisual {

        public enum TEmbed { 
            Image,
            Thumbnail,
            None
        }

        protected override string TagName => "image-block";

        public string? Alt { get; init; } = null;

        public required Genome<IAsset> AssetGenome;

        public TEmbed Embed { get; set; } = TEmbed.Thumbnail;


        public Image(Image other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
            AssetGenome = other.AssetGenome;
            Embed = other.Embed;
        }

        [SetsRequiredMembers]
        public Image(Genome<IAsset> assetGenome, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            AssetGenome = assetGenome;
        }

        [SetsRequiredMembers]
        public Image(string pathOrUrl, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            AssetGenome = Gears.AssetGenome.GenomeFromPathOrUrl(pathOrUrl, callerFilePath);
        }

        IAsset GetSource() {
            string[] webExtensions = { ".jpg", ".jpeg", ".png", ".svg" };

            var source = AssetGenome.Result;
            if (!webExtensions.Contains(source.Extension)) {
                source = new JpegGenome(AssetGenome).Result;
            }
            return source;
        }

        public override void CreateContent(Context context, Tag tag, Group scriptBeforeConstructor, Group scriptAfterConstructor, string contentId, out double width, out double height) {
            var source = AssetGenome.ToWebImage().Result;
            var imageInfo = source.GetImageInfo();
            width = imageInfo.Width;
            height = imageInfo.Height;

            string url;

            if (Embed == TEmbed.Image) {
                if (source.GetMediaType() == "image/svg+xml") {
                    url = source.GetDataUrlXml();
                } else {
                    url = source.GetDataUrlBase64();
                }
            } else {
                url = context.PathFromHostToCurrentPage.To(context.AddAsset(source)).ToString();
            }


            if (Embed == TEmbed.Thumbnail) {
                var thumbnail = new ThumbnailGenome(AssetGenome, 32).Result;
                var thumbnailUrlBase64 = thumbnail.GetDataUrlBase64();


                var thumbnailImageInfo = thumbnail.GetImageInfo();

                var thumbnailId = context.CreateId();
                context.HeadTags.Add(new Tag("link", thumbnailId) {
                    ["rel"] = "preload",
                    ["href"] = thumbnailUrlBase64,
                    ["as"] = "image",
                });
                scriptAfterConstructor.Add($"StaticSharp.SetThumbnailBackground({TagToJsValue(contentId)}, \"{thumbnailId}\", {thumbnailImageInfo.Width}, {thumbnailImageInfo.Height})");
            }

            var img = new Tag("img", contentId) {
                ["src"] = url
            };
            if (Alt != null) {
                img["alt"] = Alt;
            }
            tag.Add(img);
        }



        /*public override void ModifyTagAndScript(Context context, Tag tag, Group script) {
            base.ModifyTagAndScript(context, tag, script);

            var imgId = context.CreateId();            
            

            SetNativeSize(script, tag.Id, imageInfo.Width, imageInfo.Height);

            script.Add($"{tag.Id}.img = {TagToJsValue(imgId)}");

            

        }*/


        void IMainVisual.GetMeta(Dictionary<string, string> meta, Context context) {
            var source = AssetGenome.ToWebImage().Result;
            var imageInfo = source.GetImageInfo();
            var url = (context.BaseUrl + context.AddAsset(source)).ToString();
            meta["og:image"] = url;
            meta["og:image:width"] = imageInfo.Width.ToString();
            meta["og:image:height"] = imageInfo.Height.ToString();

            meta["twitter:image"] = url;
        }

        
    }

}