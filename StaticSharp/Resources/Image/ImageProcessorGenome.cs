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
        };

        public override Genome[]? Sources => new Genome[] { Source };

        private void SaveData(IAsset source, IAsset result) {
            Data data = new();
            data.Extension = result.FileExtension;
            data.SourceHash = source.ContentHash;
            data.ContentHash = result.ContentHash;
            var content = result.Data;

            CreateCacheSubDirectory();
            FileUtils.WriteAllBytes(ContentFilePath, content);
            StoreData(data);
        }

        public override IAsset Create() {
            Data data;
            var source = Source.CreateOrGetCached();

            if (LoadData(out data) && data.SourceHash == source.ContentHash ) {
                return new BinaryAsset(
                    FileUtils.ReadAllBytes(ContentFilePath),
                    data.Extension,
                    data.ContentHash
                    );                
            } else {
                
                var image = new MagickImage(source.Data);
                image = Process(image);

                var extension = "." + image.FormatInfo?.Format.ToString().ToLower() ?? "?";


                var result = new BinaryAsset(
                    image.ToByteArray(),
                    extension
                    );

                SaveData(source, result);

                return result;
            }
        }

        protected abstract MagickImage Process(MagickImage image);





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

