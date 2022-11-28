using ImageMagick;
using System;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Gears {
        public class MagickImageAsset : IAsset {

            Task<MagickImage> imageTask;

            byte[]? data = null;
            string? contentHash = null;

            /*string? extension;
            Task<bool> successTask;
            Task<byte[]> dataTask;
            Task<string?> mediaTypeTask;
            Task<string?> charSetTask;
            string? contentHash;*/



            public MagickImageAsset(Task<MagickImage> imageTask) {
                this.imageTask = imageTask;
            }

            public async Task<byte[]> GetBytesAsync() {
                if (data == null) {
                    var image = await imageTask;
                    data = image.ToByteArray();
                }
                return data;
            }

            public async Task<string> GetContentHashAsync() {
                if (contentHash == null) {
                    contentHash = Hash.CreateFromBytes(await GetBytesAsync()).ToString();
                }
                return contentHash;
            }

            public async Task<string> GetFileExtensionAsync() {
                var image = await imageTask;
                var extension = image.FormatInfo?.Format.ToString().ToLower() ?? "?";
                return "." + extension;
            }

            public async Task<string> GetMediaTypeAsync() {
                var image = await imageTask;

                return image.FormatInfo?.MimeType ?? "application/octet-stream";

                /*var mediaType = await mediaTypeTask;
                if (mediaType != null) {
                    return mediaType;
                }
                if (extension != null) {
                    mediaType = MimeTypeMap.GetMimeType(extension);
                    if (mediaType != null) {
                        return mediaType;
                    }
                }
                return "application/octet-stream";*/
            }

            public async Task<FilePath> GetTargetFilePathAsync() {
                return new(await GetContentHashAsync() + await GetFileExtensionAsync());
            }

            public async Task<string> GetTextAsync() {
                throw new NotImplementedException();
                /*using (MemoryStream memoryStream = new(await GetBytesAsync())) {
                    var charSet = await charSetTask;
                    Encoding encoding = charSet != null ? Encoding.GetEncoding(charSet) : Encoding.UTF8;

                    using (StreamReader streamReader = new StreamReader(memoryStream, encoding, true)) {
                        var text = streamReader.ReadToEnd();
                        return text;
                    }
                }*/
            }
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

