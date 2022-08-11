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

    namespace Symbolic {
        public class ImageJs : BlockJs {

        }
    }

    [ScriptBefore]
    [ScriptAfter]
    public class Image : Block {

        public override string TagName => "div";

        protected IGenome<IAsset> assetGenome;


        public Image(Image other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {
            assetGenome = other.assetGenome;
        }
        public Image(IGenome<IAsset> assetGenome, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            this.assetGenome = assetGenome;
        }

        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }

        
        public override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {

            var jpeg = await (new JpegGenome(assetGenome)).CreateOrGetCached();
            var url = context.AddAsset(jpeg);

            var thumbnail = await new ThumbnailGenome(assetGenome).CreateOrGetCached();

            var thumbnailId = context.SvgDefs.Add(new SvgInlineImageGenerator(new ThumbnailGenome(assetGenome)));

            var hBlurId = context.SvgDefs.Add(new SvgBlurFilterGenerator(0.5f, 0));
            var vBlurId = context.SvgDefs.Add(new SvgBlurFilterGenerator(0, 0.5f));


            var imageInfo = new MagickImageInfo(jpeg.ReadAllBites());
            elementTag["data-width"] = imageInfo.Width;
            elementTag["data-height"] = imageInfo.Height;

            elementTag.Style["background-color"] = Color.Aqua;


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