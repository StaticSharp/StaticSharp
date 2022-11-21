using ImageMagick;
using StaticSharp.Gears;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StaticSharp {





    public abstract record ImageProcessorGenome(Genome<Asset> Source) : Genome<Asset> {

        class Data {
            public string ContentHash = null!;
            public string SourceHash = null!;
            public string MimeType = null!;
            //public int Width;
            //public int Height;
        };

        public override async Task<Asset> CreateAsync() {
            Data data;
            Func<byte[]> contentCreator;
            if (!LoadData(out data)) {
                //CachedData = new();

                var source = await Source.CreateOrGetCached();
                data.SourceHash = source.ContentHash;

                var image = new MagickImage(source.ReadAllBites());

                image = await Process(image);

                //data.Width = image.Width;
                //data.Height = image.Height;

                CreateCacheSubDirectory();
                
                data.MimeType = image.FormatInfo?.MimeType ?? source.MediaType;

                var content = image.ToByteArray();
                File.WriteAllBytes(ContentFilePath, content); //TODO: mb async?
                data.ContentHash = Hash.CreateFromBytes(content).ToString();
                contentCreator = () => content;
                StoreData(data);
            } else {
                contentCreator = ()=>FileUtils.ReadAllBytes(ContentFilePath);
            }

            return new Asset(
                contentCreator,
                MimeTypes.MimeTypeMap.GetExtension(data.MimeType),
                data.MimeType,
                data.ContentHash
                );
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

