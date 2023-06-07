using ImageMagick;
using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;


namespace StaticSharp {


    public interface JImage : JAspectBlock {
        //public double Aspect  { get; }
    }

    [Scripts.FitImage]
    [ConstructorJs]
    public partial class Image : AspectBlock, IMainVisual {

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


        


        public override void ModifyTagAndScript(Context context, Tag tag, Group script) {
            base.ModifyTagAndScript(context, tag, script);

            var contentId = context.CreateId();
            var imgId = context.CreateId();
            
            var source = AssetGenome.ToWebImage().Result;
            var imageInfo = source.GetImageInfo();

            SetNativeSize(script, tag.Id, imageInfo.Width, imageInfo.Height);

            script.Add($"{tag.Id}.content = {TagToJsValue(contentId)}");


            //tag["data-width"] = imageInfo.Width;
            //tag["data-height"] = imageInfo.Height;

            string url;



            void AddSimpleImage() {
                tag.Add(new Tag("content", contentId) {
                    new Tag("img") {
                        ["width"] = "100%",
                        ["height"] = "100%",
                        ["src"] = url,
                    }
                });
            }

            if (Embed == TEmbed.Image) {
                if (source.GetMediaType() == "image/svg+xml") {
                    url = source.GetDataUrlXml();
                } else {
                    url = source.GetDataUrlBase64();
                }
                AddSimpleImage();
                return;
            }

            

            url = context.PathFromHostToCurrentPage.To(context.AddAsset(source)).ToString();
            if (Embed == TEmbed.None) {
                AddSimpleImage();
                return;
            }


            var thumbnail = new ThumbnailGenome(AssetGenome,32).Result;
            var thumbnailUrlBase64 = thumbnail.GetDataUrlBase64();


            var thumbnailImageInfo = thumbnail.GetImageInfo();

            script.Add($"{tag.Id}.thumbnailData = {{src:\"{thumbnailUrlBase64}\",width:{thumbnailImageInfo.Width},height:{thumbnailImageInfo.Height}}}");
            





            var quantizeSettings = new QuantizeSettings() {
                Colors = 4,
                ColorSpace = ColorSpace.RGB,
                DitherMethod = DitherMethod.No,
                MeasureErrors = false,
                //TreeDepth = 0
            };

            

            tag.Add(new Tag("content", contentId) {

                new Tag("img", imgId) {
                    ["width"] = "100%",
                    ["height"] = "100%",
                    ["src"] = url
                }
            }
            );

        }


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