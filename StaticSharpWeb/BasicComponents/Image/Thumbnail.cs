using ImageMagick;
using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StaticSharp {



    public record ThumbnailGenome(IGenome<IAsset> Source) : Genome<ThumbnailGenome, ThumbnailCacheable> {

    }

    namespace Gears {

        public class ThumbnailCacheable : Cacheable<ThumbnailGenome, ThumbnailCacheable.Data> {

            public class Data {
                public string ContentHash = null!;
                public string SourceHash = null!;
                public int Width;
                public int Height;
            };

            public string ImageDataBase64 { get; private set; } = null!;

            /*public string? CharSet => null;
            public string MediaType => "image/jpeg";

            public string ContentHash => CachedData.ContentHash;

            public string FileExtension => ".jpeg";

            

            public Stream CreateReadStream() {
                return File.OpenRead(ContentFilePath);
            }*/

            bool VerifyCachedData() {
                return CachedData.ContentHash != null
                    && CachedData.SourceHash != null;
            }

            private void Resize(MagickImage image, int width, int height) {
                if (image.Width > width * 6) {
                    Resize(image, 4 * width, 4 * height);
                }
                image.InterpolativeResize(width, height, PixelInterpolateMethod.Average16);
            }

            protected override async Task CreateAsync() {

                if (!LoadData() || !VerifyCachedData()) {
                    CachedData = new();

                    var source = await Genome.Source.CreateOrGetCached();

                    CachedData.SourceHash = source.ContentHash;

                    var image = new MagickImage(source.CreateReadStream());

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

                    CreateCacheSubDirectory();
                    using (var stream = new MemoryStream()) {
                        await image.WriteAsync(stream);
                        var data = stream.ToArray();
                        CachedData.ContentHash = Hash.CreateFromBytes(data).ToString();
                        ImageDataBase64 = Convert.ToBase64String(data);
                        File.WriteAllText(ContentFilePath, ImageDataBase64);
                    }

                    StoreData();

                } else {
                    ImageDataBase64 = File.ReadAllText(ContentFilePath);

                }
            }

            public Tag GetSvgTag(Context context) { 
                //var imageDataBase64 = File.ReadAllText(ContentFilePath);
                return new Tag("svg") {
                    ["viewBox"]=$"0 0 {CachedData.Width} {CachedData.Height}",
                    ["preserveAspectRatio"] = "none",
                    Children = { 
                        new Tag("filter","blur"){ 
                            new Tag("feGaussianBlur"){
                                ["stdDeviation"] = 0.5f
                            }
                        }
                    }
                };
            }
        
        }
    }

}

