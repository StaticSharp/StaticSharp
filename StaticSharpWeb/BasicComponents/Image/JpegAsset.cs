using ImageMagick;
using StaticSharp.Gears;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {


    //Instead of inheritance from CacheableHttpRequest, it is better to use aggregation



    public record JpegPromise(IPromise<IAsset> Source) : Constructor<JpegPromise, JpegAsset>, IPromise<IAsset> {
        public async Task<IAsset> GetAsync() {
            return await CreateOrGetCached();
        }
    }


    namespace Gears {

        public class JpegAsset : Cacheable<JpegPromise, JpegAsset.Data>, IAsset {

            public class Data {
                public string ContentHash = null!;
                public string SourceHash = null!;
            };

            public string? CharSet => null;
            public string MediaType => "image/jpeg";

            

            public string ContentHash => CachedData.ContentHash;

            public string FileExtension => ".jpeg";

            public byte[] Content { get; private set; } = null!;

            public Stream CreateReadStream() {
                return File.OpenRead(ContentFilePath);
            }

            bool VerifyCachedData() {
                return CachedData.ContentHash != null
                    && CachedData.SourceHash != null;
            }

            protected override async Task CreateAsync() {

                if (!LoadData() || !VerifyCachedData()) {
                    CachedData = new();

                    var source = await Arguments.Source.GetAsync();

                    CachedData.SourceHash = source.ContentHash;

                    var image = new MagickImage(source.CreateReadStream());
                    
                    if (image.Format != MagickFormat.Jpeg) {
                        image.Quality = 85;
                    }

                    //https://stackoverflow.com/questions/19788127/save-a-jpeg-in-progressive-format
                    image.Format = MagickFormat.Pjpeg;
                    image.Strip();

                    CreateCacheSubDirectory();
                    using (var stream = new MemoryStream()) {
                        await image.WriteAsync(stream);
                        CachedData.ContentHash = Hash.CreateFromStream(stream).ToString();
                        using (var fileStream = File.Create(ContentFilePath)) {
                            stream.Seek(0, SeekOrigin.Begin);
                            stream.CopyTo(fileStream);


                            fileStream.Seek(0, SeekOrigin.Begin);
                            var imageCheck = new MagickImage(fileStream);


                            fileStream.Close();
                        }
                    }

                    


                    StoreData();
                }
            }
        }
    }
}

