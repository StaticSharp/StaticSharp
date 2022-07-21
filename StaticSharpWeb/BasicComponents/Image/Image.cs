using ImageMagick;
using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Symbolic {
        public class ImageJs : BlockJs {

        }
    }


    public abstract class Image<Js> : Block<Js> where Js : Symbolic.ImageJs, new() {

        protected IPromise<IAsset> assetPromise;


        public Image(Image<Js> other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {
            assetPromise = other.assetPromise;
        }
        public Image(IPromise<IAsset> assetPromise, string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) {
            this.assetPromise = assetPromise;
        }

        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }
    }

    [ScriptBefore]
    [ScriptAfter]
    public sealed class Image : Image<Symbolic.ImageJs> {

        public Image(Image other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {

        }

        /*public Image(string filePath, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(filePath, callerFilePath, callerLineNumber) {
            
        }*/

        public Image(IPromise<IAsset> assetPromise, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(assetPromise, callerFilePath, callerLineNumber) {

        }



        public override string TagName => "div";
        public override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            var asset = await assetPromise.GetAsync();
            var url = context.AddAsset(asset);

            /*Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var s = 0;
            for (int i = 0; i < 0xff; i++) {
                var imageInfo = new MagickImageInfo(asset.CreateReadStream());
                s += imageInfo.Width;
            }
            Console.WriteLine(stopWatch.ElapsedMilliseconds);

            stopWatch.Start();
            for (int i = 0; i < 0xff; i++) {
                var image = new MagickImage(asset.CreateReadStream());
                s += image.Width;
            }
            Console.WriteLine(stopWatch.ElapsedMilliseconds);
            Console.WriteLine(s);*/

            var image = new MagickImage(asset.CreateReadStream());

            var imageInfo = new MagickImageInfo(asset.CreateReadStream());
            image.InterpolativeResize(512, 512, PixelInterpolateMethod.Average16);
            image.InterpolativeResize(128, 128, PixelInterpolateMethod.Average16);
            image.InterpolativeResize(32, 32, PixelInterpolateMethod.Average16);
            //image.InterpolativeResize(16, 16, PixelInterpolateMethod.Average16);

            image.Format = MagickFormat.Jpeg;
            image.Quality = 90;
            image.Strip();
            string base64Thumbnail;
            using (var memoryStream = new MemoryStream()) { 
                image.Write(memoryStream);
                Console.WriteLine(memoryStream.Length);
                base64Thumbnail = Convert.ToBase64String(memoryStream.ToArray());
            }



            elementTag["data-width"] = imageInfo.Width;
            elementTag["data-height"] = imageInfo.Height;

            return new Tag("img") {

                ["src"] = $"data:image/jpg;base64,{base64Thumbnail}"
            };
        }
    }
}