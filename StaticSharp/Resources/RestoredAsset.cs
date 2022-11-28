using System.Text;

namespace StaticSharp {

    namespace Gears {


        public class TextAsset : AssetSync {

            string extension;
            string mediaType;
            string? contentHash;
            string text;

            public TextAsset(string text, string extension, string mediaType, string? contentHash = null) {
                this.extension = extension;
                this.text = text;
                this.mediaType = mediaType;
                this.contentHash = contentHash;
            }

            public override string GetFileExtension() => extension;
            public override string GetMediaType() => mediaType;
            public override string GetContentHash() {
                if (contentHash == null) {
                    contentHash = Hash.CreateFromString(text).ToString();
                }
                return contentHash;
            }
            public override byte[] GetBytes() => Encoding.UTF8.GetBytes(text);
            public override string GetText() => text;

        }



        public class RestoredAsset : AssetSync {

            string extension;
            string mediaType;
            string contentHash;

            byte[] data;

            public RestoredAsset(string extension, string mediaType, string contentHash, byte[] data) {
                this.extension = extension;
                this.data = data;
                this.mediaType = mediaType;
                this.contentHash = contentHash;
            }

            public override string GetFileExtension() => extension;
            public override string GetMediaType() => mediaType;
            public override string GetContentHash() => contentHash;
            public override byte[] GetBytes() => data;
            public override string GetText() => Encoding.UTF8.GetString(data);

        }




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

