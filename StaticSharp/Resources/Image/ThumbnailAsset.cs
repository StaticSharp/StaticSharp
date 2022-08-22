using ImageMagick;
using StaticSharp.Gears;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StaticSharp {

    public record ThumbnailGenome(IGenome<IAsset> Source) : ImageProcessorGenome<ThumbnailGenome, ThumbnailAsset>(Source) {}


    namespace Gears {
        public class ThumbnailAsset : ImageProcessorAsset<ThumbnailGenome, ImageProcessorAssetData>, IImageAsset {
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

                CachedData.Width = (int)Math.Round(image.Width * scale);
                CachedData.Height = (int)Math.Round(image.Height * scale);



                var numDownscales = (int)Math.Truncate(0.5f * Math.Log2(image.Width / (float)size));

                MagickGeometry geometry = new MagickGeometry();
                geometry.IgnoreAspectRatio = true;


                for (int i = numDownscales; i >= 1; i--) {
                    geometry.Width = CachedData.Width * (1 << (2 * i));
                    geometry.Height = CachedData.Height * (1 << (2 * i));

                    image.InterpolativeResize(geometry, PixelInterpolateMethod.Average16);
                }
                geometry.Width = CachedData.Width;
                geometry.Height = CachedData.Height;
                image.InterpolativeResize(geometry, PixelInterpolateMethod.Average16);

                image.Quality = 50;
                image.Format = MagickFormat.Pjpeg;
                image.Strip();

                return Task.FromResult(image);

            }
        }
    }

}

