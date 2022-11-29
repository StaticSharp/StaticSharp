using ImageMagick;
using StaticSharp.Gears;
using System.IO;
using System.Threading.Tasks;

namespace StaticSharp {





    public abstract record ImageProcessorGenome(Genome<IAsset> Source) : Genome<IAsset> {

        class Data {
            public string Extension = null!;
            public string ContentHash = null!;
            public string SourceHash = null!;
            public string MimeType = null!;
            //public int Width;
            //public int Height;
        };

        public override Genome[]? Sources => new Genome[] { Source };

        private async Task SaveDataAsync(IAsset source, IAsset result) {
            Data data = new();
            data.Extension = await result.GetFileExtensionAsync();
            data.SourceHash = await source.GetContentHashAsync();
            data.ContentHash = await result.GetContentHashAsync();
            data.MimeType = await result.GetMediaTypeAsync();
            var content = await result.GetBytesAsync();

            CreateCacheSubDirectory();
            await FileUtils.WriteAllBytesAsync(ContentFilePath, content);
            StoreData(data);
        }

        private async Task<MagickImage> LoadAndProcessAsync(IAsset source) {
            var image = new MagickImage(await source.GetBytesAsync());
            image = await Process(image);
            return image;
        }

        public override IAsset Create() {
            Data data;
            if (!LoadData(out data)) {

                var source = Source.CreateOrGetCached();
                var result = new MagickImageAsset(LoadAndProcessAsync(source));
                _ = SaveDataAsync(source,result);

                return result;
            } else {
                return new RestoredAsset(
                    data.Extension,
                    data.MimeType,
                    data.ContentHash,
                    FileUtils.ReadAllBytes(ContentFilePath)
                    );
            }
        }

        protected abstract Task<MagickImage> Process(MagickImage image);





    }

    namespace Gears {




        /*public abstract class ImageProcessorAsset<TGenome> : ImageAsset<TGenome>, IMutableAsset
            where TGenome : class, IKeyProvider, IImageProcessorGenome {

            class Data {
                public string ContentHash = null!;
                public string SourceHash = null!;
                public int Width;
                public int Height;
            };
            public abstract string MediaType { get; }

            private Data data = null!;
            public string ContentHash => data.ContentHash;

            public abstract string FileExtension { get; }

            public int Width => data.Width;
            public int Height => data.Height;

            public async Task<bool> GetValidAsync() {
                var source = await Genome.Source.CreateOrGetCached();
                return source.ContentHash == data.SourceHash;
            }

            bool VerifyCachedData() {
                return data.ContentHash != null
                    && data.SourceHash != null;
            }


            protected abstract Task<MagickImage> Process(MagickImage source);

            protected override async Task CreateAsync() {

                if (!LoadData(out data) || !VerifyCachedData()) {
                    //CachedData = new();

                    var source = await Genome.Source.CreateOrGetCached();
                    data.SourceHash = source.ContentHash;

                    var image = new MagickImage(source.ReadAllBites());

                    image = await Process(image);

                    data.Width = image.Width;
                    data.Height = image.Height;                    

                    CreateCacheSubDirectory();

                    Content = image.ToByteArray();
                    File.WriteAllBytes(ContentFilePath, Content); //TODO: mb async?
                    data.ContentHash = Hash.CreateFromBytes(Content).ToString();

                    StoreData(data);
                }
            }


        }*/
    }
}

