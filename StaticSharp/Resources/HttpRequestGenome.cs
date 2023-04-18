using MimeTypes;
using StaticSharp.Gears;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
                var userAgent = HttpRequestMessage.Headers.UserAgent;
                if (userAgent.Count == 0) {
                    userAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("StaticSharp")));
                }
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

