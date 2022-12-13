using MimeTypes;
using StaticSharp.Gears;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Resources {
        public static partial class Static {

            public static IAsset LoadHttp(string uri) {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                return LoadHttp(httpRequestMessage);
            }
            public static IAsset LoadHttp(HttpRequestMessage httpRequestMessage) {
                return new HttpRequestGenome(httpRequestMessage).Result;
            }

        }
    
    
    }



    public record HttpRequestGenome(HttpRequestMessage HttpRequestMessage) : Genome<IAsset> {
        public HttpRequestGenome(string uri) : this(new Uri(uri)) { }
        public HttpRequestGenome(Uri uri) : this(new HttpRequestMessage(HttpMethod.Get, uri)) { }

        class Data {
            public string Extension = null!;
            public string ContentHash = null!;
        };

        private async Task SaveDataAsync(Cache.Slot slot, AssetAsyncData asset) {
            Data data = new();
            data.Extension = asset.Extension;
            data.ContentHash = await asset.GetContentHashAsync();
            var content = await asset.GetDataAsync();

            slot
                .StoreContent(content)
                .StoreData(data);
        }


        protected override void Create(out IAsset value, out Func<bool>? verify) {
            Data data;

            var path = HttpRequestMessage.RequestUri?.AbsolutePath;
            var extension = Path.GetExtension(path);

            verify = null;
            var slot = Cache.GetSlot(Key);
            if (!slot.LoadData(out data)) {

                var response = HttpClientStatic.Instance.Send(HttpRequestMessage);
                var mediaType = response.Content.Headers.ContentType?.MediaType;
                if (mediaType != null) {
                    try {
                        extension = MimeTypeMap.GetExtension(mediaType);
                    }
                    catch (Exception) { }
                }

                var result = new AssetAsyncData(
                    response.Content.ReadAsByteArrayAsync(),
                    extension ?? ".?"
                    );

                _ = SaveDataAsync(slot,result);
                value = result;

            } else {
                value = new BinaryAsset(
                    slot.LoadContent(),
                    data.Extension,
                    data.ContentHash
                    );
            }
        }
    }



/*


        public class HttpRequestAsset : IAssetDataAsync {


            string? contentHash = null;
            HttpRequestMessage httpRequestMessage;
            Task<HttpResponseMessage> httpResponseMessageTask;

            public HttpRequestAsset(HttpRequestMessage httpRequestMessage) {
                this.httpRequestMessage = httpRequestMessage;
                httpResponseMessageTask = HttpClientStatic.Instance.SendAsync(httpRequestMessage);
            }





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
*/
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

