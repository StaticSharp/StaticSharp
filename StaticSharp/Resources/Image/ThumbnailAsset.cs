using ImageMagick;
using StaticSharp.Gears;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StaticSharp {

    public record ThumbnailGenome(Genome<Asset> Source) : ImageProcessorGenome(Source) {
        protected override Task<MagickImage> Process(MagickImage image) {
            var size = 32;
            var maxDimension = Math.Max(image.Width, image.Height);
            var scale = size / (float)maxDimension;

            var finalWidth = (int)Math.Round(image.Width * scale);
            var finalHeight = (int)Math.Round(image.Height * scale);



            var numDownscales = (int)Math.Truncate(0.5f * Math.Log2(image.Width / (float)size));

            MagickGeometry geometry = new MagickGeometry();
            geometry.IgnoreAspectRatio = true;


            for (int i = numDownscales; i >= 1; i--) {
                geometry.Width = finalWidth * (1 << (2 * i));
                geometry.Height = finalHeight * (1 << (2 * i));

                image.InterpolativeResize(geometry, PixelInterpolateMethod.Average16);
            }
            geometry.Width = finalWidth;
            geometry.Height = finalHeight;
            image.InterpolativeResize(geometry, PixelInterpolateMethod.Average16);

            image.Quality = 50;
            image.Format = MagickFormat.Pjpeg;
            image.Strip();

            return Task.FromResult(image);

        }

    }


    /*namespace Gears {
        public class ThumbnailAsset : ImageProcessorAsset<ThumbnailGenome>, IImageAsset {
            public override string MediaType => "image/jpeg";
            public override string FileExtension => ".jpg";

            private void Resize(MagickImage image, int width, int height) {
                if (image.Width > width * 6) {
                    Resize(image, 4 * width, 4 * height);
                }
                image.InterpolativeResize(width, height, PixelInterpolateMethod.Average16);
            }

            protected override Task<MagickImage> Process(MagickImage image) {
                var size = 32;
                var maxDimension = Math.Max(image.Width, image.Height);
                var scale = size / (float)maxDimension;

                var finalWidth = (int)Math.Round(image.Width * scale);
                var finalHeight = (int)Math.Round(image.Height * scale);



                var numDownscales = (int)Math.Truncate(0.5f * Math.Log2(image.Width / (float)size));

                MagickGeometry geometry = new MagickGeometry();
                geometry.IgnoreAspectRatio = true;


                for (int i = numDownscales; i >= 1; i--) {
                    geometry.Width = finalWidth * (1 << (2 * i));
                    geometry.Height = finalHeight * (1 << (2 * i));

                    image.InterpolativeResize(geometry, PixelInterpolateMethod.Average16);
                }
                geometry.Width = finalWidth;
                geometry.Height = finalHeight;
                image.InterpolativeResize(geometry, PixelInterpolateMethod.Average16);

                image.Quality = 50;
                image.Format = MagickFormat.Pjpeg;
                image.Strip();

                return Task.FromResult(image);

            }
        }
    }*/

}

