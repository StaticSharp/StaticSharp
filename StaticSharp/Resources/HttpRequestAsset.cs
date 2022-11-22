using MimeTypes;
using StaticSharp.Gears;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {


    public record HttpRequestGenome(HttpRequestMessage HttpRequestMessage) : Genome<IAsset> {
        public HttpRequestGenome(string uri) : this(new Uri(uri)) { }
        public HttpRequestGenome(Uri uri) : this(new HttpRequestMessage(HttpMethod.Get, uri) {

        }) {}

        class Data {
            public string? CharSet;
            public string MediaType = null!;
            public string ContentHash = null!;
            //public string Extension = null!;
        };



        private async Task SaveDataAsync(HttpRequestAsset asset) {
            Data data = new();
            data.CharSet = await asset.GetCharSetAsync();
            data.ContentHash = await asset.GetContentHashAsync();
            data.MediaType = await asset.GetMediaTypeAsync();
            var content = await asset.GetBytesAsync();
            CreateCacheSubDirectory();
            await FileUtils.WriteAllBytesAsync(ContentFilePath, content);
            StoreData(data);
        }

        public override async Task<IAsset> CreateAsync() {
            Data data;
            Func<byte[]> contentCreator = null!;

            var path = HttpRequestMessage.RequestUri?.AbsolutePath;
            var extension = Path.GetExtension(path);


            if (!LoadData(out data)) {
                var httpResponseMessageTask = HttpClientStatic.Instance.SendAsync(HttpRequestMessage);

                var successTask = httpResponseMessageTask.ContinueWith(x=>x.Result.IsSuccessStatusCode);



                /*if (!httpResponseMessage.IsSuccessStatusCode) {
                    throw new Exception($"Failed to get {HttpRequestMessage.RequestUri} with code {httpResponseMessage.StatusCode}");
                    //FIXME:
                    //appears as
                    //One or more errors occurred. (Failed to get https://raw.githubusercontent.com/Templarian/MaterialDesign/master/svg/book-variant-multiple.svg with code NotFound)Failed to get https://raw.githubusercontent.com/Templarian/MaterialDesign/master/svg/book-variant-multiple.svg with code NotFound
                }*/

                var result = new HttpRequestAsset(
                    extension,
                    successTask,
                    httpResponseMessageTask.ContinueWith(x => x.Result.Content.ReadAsByteArrayAsync()).ContinueWith(x => x.Result.Result),
                    httpResponseMessageTask.ContinueWith(x => x.Result.Content.Headers.ContentType?.MediaType),
                    httpResponseMessageTask.ContinueWith(x => x.Result.Content.Headers.ContentType?.CharSet),
                    null
                    );

                _ = SaveDataAsync(result);

                return result;


                /*data.CharSet = (httpResponseMessage.Content.Headers.ContentType?.CharSet);

                var mediaType = httpResponseMessage.Content.Headers.ContentType?.MediaType;
                var path = HttpRequestMessage.RequestUri?.AbsolutePath;
                var extension = Path.GetExtension(path);
                if (string.IsNullOrEmpty(extension))
                    extension = null;

                if (mediaType != null) {
                    if (extension == null) {
                        extension = MimeTypeMap.GetExtension(mediaType, false);
                    }
                } else {
                    mediaType = MimeTypeMap.GetMimeType(extension);
                }

                if (extension == null) {
                    extension = ".unknown";
                }

                data.MediaType = mediaType;
                data.Extension = extension;

                CreateCacheSubDirectory();

                var content = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                contentCreator = () => content;

                data.ContentHash = Hash.CreateFromBytes(content).ToString();
                await FileUtils.WriteAllBytesAsync(ContentFilePath, content);
                StoreData(data);*/

            } else {

                var result = new HttpRequestAsset(
                    extension,
                    Task.FromResult(true),
                    Task.FromResult(FileUtils.ReadAllBytes(ContentFilePath)),
                    Task.FromResult<string?>(data.MediaType),
                    Task.FromResult(data.CharSet),
                    data.ContentHash
                    );


                //contentCreator = ()=> FileUtils.ReadAllBytes(ContentFilePath);
            }

            /*return new Asset(
                contentCreator,
                data.Extension,
                data.MediaType,
                data.ContentHash,
                data.CharSet
                );*/
        }

        public class HttpRequestAsset : IAsset {

            string? extension;
            Task<bool> successTask;
            Task<byte[]> dataTask;
            Task<string?> mediaTypeTask;
            Task<string?> charSetTask;
            string? contentHash;
            public HttpRequestAsset(string? extension, Task<bool> successTask, Task<byte[]> dataTask, Task<string?> mediaTypeTask, Task<string?> charSetTask, string? contentHash) {
                this.successTask = successTask;
                this.extension = extension;
                this.dataTask = dataTask;
                this.mediaTypeTask = mediaTypeTask;
                this.charSetTask = charSetTask;
                this.contentHash = contentHash;
            }

            public async Task<string?> GetCharSetAsync() {
                return await charSetTask;
            }

            public Task<byte[]> GetBytesAsync() {
                return dataTask;
            }

            public async Task<string> GetContentHashAsync() {
                if (contentHash == null) {
                    contentHash = Hash.CreateFromBytes(await GetBytesAsync()).ToString();
                }
                return contentHash;
            }

            public async Task<string> GetFileExtensionAsync() {
                if (extension != null)
                    return extension;

                var mediaType = await mediaTypeTask;
                if (mediaType != null) {
                    return MimeTypeMap.GetExtension(mediaType);
                }
                
                return ".?";
            }

            public async Task<string> GetMediaTypeAsync() {
                var mediaType = await mediaTypeTask;
                if (mediaType != null) {
                    return mediaType;
                }
                if (extension != null) {
                    mediaType = MimeTypeMap.GetMimeType(extension);
                    if (mediaType != null) {
                        return mediaType;
                    }
                }
                return "application/octet-stream";
            }

            public async Task<FilePath> GetTargetFilePathAsync() {
                return new(await GetContentHashAsync() + await GetFileExtensionAsync());
            }

            public async Task<string> GetTextAsync() {
                using (MemoryStream memoryStream = new(await GetBytesAsync())) {
                    var charSet = await charSetTask;
                    Encoding encoding = charSet!=null ? Encoding.GetEncoding(charSet) : Encoding.UTF8;

                    using (StreamReader streamReader = new StreamReader(memoryStream, encoding, true)) {
                        var text = streamReader.ReadToEnd();
                        return text;
                    }
                }
            }
        }



    }

    //Instead of inheritance from CacheableHttpRequest, it is better to use aggregation
    namespace Gears {

        public static partial class KeyCalculators {
            public static string GetKey(HttpRequestMessage httpRequestMessage) {

                var headers = string.Join('\0', httpRequestMessage.Headers.Select(x => $"{x.Key}\0{string.Join(',', x.Value)}"));
                
                return KeyUtils.Combine<HttpRequestMessage>(
                    httpRequestMessage.RequestUri?.ToString(),
                    httpRequestMessage.Method.ToString(),
                    headers
                    );
            }
        }



       /* public class HttpRequestAsset : CacheableToFile<HttpRequestGenome>, Asset {

            class Data {
                public string? CharSet;
                public string MediaType = null!;
                public string ContentHash = null!;
                public string Extension = null!;
            };


            public static readonly string DefaultMediaType = "application/octet-stream";

            private Data data = null!;
            public override string? CharSet  => data.CharSet;
            public string MediaType => data.MediaType!=null ? data.MediaType : DefaultMediaType;            
            public string ContentHash => data.ContentHash;
            public string FileExtension => data.Extension;

            protected override void SetGenome(HttpRequestGenome arguments) {
                base.SetGenome(arguments);

                var uri = Genome.HttpRequestMessage.RequestUri;
                if (uri == null) {
                    throw new ArgumentException("HttpRequestMessage.RequestUri");
                }
            }

            protected override async Task CreateAsync() {


                



            }

            
            
        }*/

    }
}

