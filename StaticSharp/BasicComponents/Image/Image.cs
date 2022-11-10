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
        public class ImageJs : Block {

        }
    }

    [ConstructorJs]
    public class Image : Block, IMainVisual {

        public enum TEmbed { 
            Image,
            Thumbnail,
            None
        }

        protected override string TagName => "div";

        protected IGenome<IAsset> assetGenome;

        public TEmbed Embed { get; set; } = TEmbed.Thumbnail;


        public Image(Image other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {
            assetGenome = other.assetGenome;
        }
        public Image(IGenome<IAsset> assetGenome, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            this.assetGenome = assetGenome;
        }
        
        public Image(string pathOrUrl, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            if (File.Exists(pathOrUrl)) {
                assetGenome = new FileGenome(pathOrUrl);
                return;
            }
            var absolutePath = AbsolutePath(pathOrUrl, callerFilePath);
            if (File.Exists(absolutePath)) {
                assetGenome = new FileGenome(absolutePath);
                return;
            }
            if (Uri.TryCreate(pathOrUrl, UriKind.Absolute, out var uri)) {
                assetGenome = new HttpRequestGenome(uri.ToString());
                return;
            }

            //TODO: 
            throw new FileNotFoundException("File or Url not found", pathOrUrl);
        }

        /*public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }*/
        async Task<IAsset> GetSourceAsync() {
            string[] webExtensions = { ".jpg", ".jpeg", ".png", ".svg" };

            var source = await assetGenome.CreateOrGetCached();
            if (!webExtensions.Contains(source.FileExtension)) {
                source = await new JpegGenome(assetGenome).CreateOrGetCached();
            }
            return source;
        }

        MagickImageInfo GetImageInfo(IAsset source) {
            return new MagickImageInfo(source.ReadAllBites());
        }

        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {

            var source = await GetSourceAsync();
            var imageInfo = GetImageInfo(source);
            elementTag["data-width"] = imageInfo.Width;
            elementTag["data-height"] = imageInfo.Height;

            string url;


            if (Embed == TEmbed.Image) {
                if (source.MediaType == "image/svg+xml") {
                    url = source.GetDataUrlXml();
                } else {
                    url = source.GetDataUrlBase64();
                }
                elementTag.Add(new Tag("img") {
                    ["src"] = url
                });
                return;
            }

            url = await context.AddAssetAsync(source);
            if (Embed == TEmbed.None) {
                elementTag.Add(new Tag("img") {
                    ["src"] = url
                });
                return;
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

            elementTag.Add(new Tag("content") {
                new Tag("svg"){
                    ["width"] = "100%",
                    ["height"] = "100%",
                    ["viewBox"] = $"0 0 {thumbnail.Width} {thumbnail.Height}",
                    ["preserveAspectRatio"] = "none",
                    Style = {
                        ["overflow"] = "hidden",
                    },
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
            }
            );

            await base.ModifyHtmlAsync(context, elementTag);

        }

        async Task IMainVisual.GetMetaAsync(Dictionary<string, string> meta, Context context) {
            var source = await GetSourceAsync();
            var imageInfo = GetImageInfo(source);
            var url = new Uri(context.BaseUrl, await context.AddAssetAsync(source)).ToString();
            meta["og:image"] = url;
            meta["og:image:width"] = imageInfo.Width.ToString();
            meta["og:image:height"] = imageInfo.Height.ToString();

            meta["twitter:image"] = url;
        }
    }

}