using ImageMagick;
using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Gears {
        public class ImageJs : BlockJs {

        }
    }

    [RelatedScript]
    public class Image : Block {

        public enum TEmbed { 
            Image,
            Thumbnail,
            None
        }

        public override string TagName => "div";

        protected IGenome<IAsset> assetGenome;

        public TEmbed Embed { get; set; } = TEmbed.Thumbnail;


        public Image(Image other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {
            assetGenome = other.assetGenome;
        }
        public Image(IGenome<IAsset> assetGenome, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            this.assetGenome = assetGenome;
        }

        /*public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }*/


        protected override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            string[] webExtensions = { ".jpg", ".jpeg", ".png", ".svg" };

            var source = await assetGenome.CreateOrGetCached();
            if (!webExtensions.Contains(source.FileExtension)) {
                source = await (new JpegGenome(assetGenome)).CreateOrGetCached();
            }

            var imageInfo = new MagickImageInfo(source.ReadAllBites());
            elementTag["data-width"] = imageInfo.Width;
            elementTag["data-height"] = imageInfo.Height;

            string url = "";


            if (Embed == TEmbed.Image) {
                if (source.MediaType == "image/svg+xml") {
                    url = source.GetDataUrlXml();
                } else {
                    url = source.GetDataUrlBase64();
                }
                return new Tag("img") {
                    ["src"] = url
                };
            }

            url = context.AddAsset(source).ToString();
            if (Embed == TEmbed.None) {
                return new Tag("img") {
                    ["src"] = url
                };
            }


            var thumbnail = await new ThumbnailGenome(assetGenome).CreateOrGetCached();
            var thumbnailId = context.SvgDefs.Add(new SvgInlineImageGenerator(new ThumbnailGenome(assetGenome)));
            var hBlurId = context.SvgDefs.Add(new SvgBlurFilterGenerator(0.5f, 0));
            var vBlurId = context.SvgDefs.Add(new SvgBlurFilterGenerator(0, 0.5f));


            var quantizeSettings = new QuantizeSettings() {
                Colors = 4,
                ColorSpace = ColorSpace.RGB,
                DitherMethod = DitherMethod.No,
                MeasureErrors = false,
                //TreeDepth = 0
            };


            /*var imageColor = new MagickImage(thumbnail.ReadAllBites());
            imageColor.Quantize(quantizeSettings);

            var colors = imageColor.UniqueColors();

            var pixel = imageColor.GetPixels().First().ToColor().ToHexString();*/


            return new Tag("content") {
                new Tag("svg"){
                    ["width"] = "100%",
                    ["height"] = "100%",
                    ["viewBox"] = $"0 0 {thumbnail.Width} {thumbnail.Height}",
                    ["preserveAspectRatio"] = "none",
                    Children = {
                        new Tag("use"){
                            ["href"]="#"+thumbnailId,
                        },
                        new Tag("use"){
                            ["href"]="#"+thumbnailId,
                            ["filter"] = $"url(#{vBlurId})"
                        },
                        new Tag("g"){
                            ["filter"] = $"url(#{hBlurId})",
                            Children = {
                                new Tag("use"){
                                    ["href"]="#"+thumbnailId,
                                },
                                new Tag("use"){
                                    ["href"]="#"+thumbnailId,
                                    ["filter"] = $"url(#{vBlurId})"
                                }
                            }
                        }
                    }
                },
                new Tag("img") {
                    ["width"] = "100%",
                    ["height"] = "100%",
                    ["src"] = url
                }
            };


            /*;*/
        }

    }

    
    /*public sealed class Image : Image<Symbolic.ImageJs> {

        public Image(Image other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {

        }

        public Image(IGenome<IAsset> assetGenome, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(assetGenome, callerFilePath, callerLineNumber) {

        }
        
    }*/
}