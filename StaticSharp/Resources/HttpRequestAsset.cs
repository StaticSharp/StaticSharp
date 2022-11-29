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
        public HttpRequestGenome(Uri uri) : this(new HttpRequestMessage(HttpMethod.Get, uri)) {}

        class Data {
            public string? CharSet;
            public string MediaType = null!;
            public string ContentHash = null!;
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

        public override IAsset Create() {
            Data data;


            var path = HttpRequestMessage.RequestUri?.AbsolutePath;
            var extension = Path.GetExtension(path);

            if (!LoadData(out data)) {
                //var httpResponseMessageTask = HttpClientStatic.Instance.SendAsync(HttpRequestMessage);
                //var successTask = httpResponseMessageTask.ContinueWith(x=>x.Result.IsSuccessStatusCode);

                /*if (!httpResponseMessage.IsSuccessStatusCode) {
                    throw new Exception($"Failed to get {HttpRequestMessage.RequestUri} with code {httpResponseMessage.StatusCode}");
                    //FIXME:
                    //appears as
                    //One or more errors occurred. (Failed to get https://raw.githubusercontent.com/Templarian/MaterialDesign/master/svg/book-variant-multiple.svg with code NotFound)Failed to get https://raw.githubusercontent.com/Templarian/MaterialDesign/master/svg/book-variant-multiple.svg with code NotFound
                }*/

                var result = new HttpRequestAsset(
                    HttpRequestMessage
                    /*extension,
                    successTask,
                    httpResponseMessageTask.ContinueWith(x => x.Result.Content.ReadAsByteArrayAsync()).ContinueWith(x => x.Result.Result),
                    httpResponseMessageTask.ContinueWith(x => x.Result.Content.Headers.ContentType?.MediaType),
                    httpResponseMessageTask.ContinueWith(x => x.Result.Content.Headers.ContentType?.CharSet),
                    null*/
                    );

                _ = SaveDataAsync(result);

                return result;

            } else {

                return new RestoredAsset(
                    extension,
                    data.MediaType,
                    data.ContentHash,
                    FileUtils.ReadAllBytes(ContentFilePath)
                    );
            }
        }

        public class HttpRequestAsset : IAsset {

            /*string? extension;
            Task<bool> successTask;
            Task<byte[]> dataTask;
            Task<string?> mediaTypeTask;
            Task<string?> charSetTask;*/

            string? contentHash = null;


            HttpRequestMessage httpRequestMessage;
            Task<HttpResponseMessage> httpResponseMessageTask;

            public HttpRequestAsset(HttpRequestMessage httpRequestMessage) {
                this.httpRequestMessage = httpRequestMessage;
                httpResponseMessageTask = HttpClientStatic.Instance.SendAsync(httpRequestMessage);
            }

            /*public HttpRequestAsset(string? extension, Task<bool> successTask, Task<byte[]> dataTask, Task<string?> mediaTypeTask, Task<string?> charSetTask, string? contentHash) {
                this.successTask = successTask;
                this.extension = extension;
                this.dataTask = dataTask;
                this.mediaTypeTask = mediaTypeTask;
                this.charSetTask = charSetTask;
                this.contentHash = contentHash;
            }*/

            public async Task<string?> GetCharSetAsync() {
                return (await httpResponseMessageTask).Content.Headers.ContentType?.CharSet;
                //return await charSetTask;
            }

            public async Task<byte[]> GetBytesAsync() {
                return await (await httpResponseMessageTask).Content.ReadAsByteArrayAsync();
            }

            public async Task<string> GetContentHashAsync() {
                if (contentHash == null) {
                    contentHash = Hash.CreateFromBytes(await GetBytesAsync()).ToString();
                }
                return contentHash;
            }


            private string? RawExtension() {
                var path = httpRequestMessage.RequestUri?.AbsolutePath;
                var extension = Path.GetExtension(path);
                return extension;
            }

            private async Task<string?> RawMediaTypeAsync() {
                var mediaType = (await httpResponseMessageTask).Content.Headers.ContentType?.MediaType;
                return mediaType;
            }


            public async Task<string> GetFileExtensionAsync() {
                var extension = RawExtension();

                if (extension != null)
                    return extension;

                var mediaType = await RawMediaTypeAsync();
                if (mediaType != null) {
                    return MimeTypeMap.GetExtension(mediaType);
                }
                
                return ".?";
            }

            public async Task<string> GetMediaTypeAsync() {
                var mediaType = await RawMediaTypeAsync();
                if (mediaType != null) {
                    return mediaType;
                }
                var extension = RawExtension();
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
                    var charSet = await GetCharSetAsync();
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
    }
}

