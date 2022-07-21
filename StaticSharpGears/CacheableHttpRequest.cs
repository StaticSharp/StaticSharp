using MimeTypes;
using System.Net.Http.Headers;
using System.Text;

namespace StaticSharp {


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



       public record HttpRequest(HttpRequestMessage HttpRequestMessage) : Constructor<HttpRequest, CacheableHttpRequest> , IPromise<IAsset> {
            public HttpRequest(string uri) : this(new Uri(uri)) { }
            public HttpRequest(Uri uri) : this(new HttpRequestMessage(HttpMethod.Get, uri) {

            }) { }

            public async Task<IAsset> GetAsync() {
                return await CreateOrGetCached();
            }
        }

    


        public class CacheableHttpRequest : Cacheable<HttpRequest, CacheableHttpRequest.Data>, IAsset {



            public class Data {
                public string? CharSet;
                public string MediaType = null!;
                public string ContentHash = null!;
                public string Extension = null!;
            };


            public static readonly string DefaultMediaType = "application/octet-stream";

            public string? CharSet => CachedData.CharSet;
            public string MediaType => CachedData.MediaType!=null ? CachedData.MediaType : DefaultMediaType;
            public byte[] Content { get; private set; } = null!;
            public string ContentHash => CachedData.ContentHash;

            private string ContentFilePath => Path.Combine(CacheSubDirectory, "content");

            protected override void SetArguments(HttpRequest arguments) {
                base.SetArguments(arguments);

                var uri = Arguments.HttpRequestMessage.RequestUri;
                if (uri == null) {
                    throw new ArgumentException("HttpRequestMessage.RequestUri");
                }
            }



            protected override async Task CreateAsync() {


                if (!LoadData()) {
                    CachedData = new();

                    var httpResponseMessage = await HttpClientStatic.Instance.SendAsync(Arguments.HttpRequestMessage);
                    if (!httpResponseMessage.IsSuccessStatusCode) {
                        throw new Exception(); //TODO: details
                    }

                    CachedData.CharSet = (httpResponseMessage.Content.Headers.ContentType?.CharSet);

                    var mediaType = httpResponseMessage.Content.Headers.ContentType?.MediaType;
                    var path = Arguments.HttpRequestMessage.RequestUri?.AbsolutePath;
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

                    CachedData.MediaType = mediaType;
                    CachedData.Extension = extension;

                    CreateCacheSubDirectory();

                    Content = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                    CachedData.ContentHash = Gears.Hash.CreateFromBytes(Content).ToString();

                    await File.WriteAllBytesAsync(ContentFilePath, Content);

                    StoreData();

                } else {
                    Content = await File.ReadAllBytesAsync(ContentFilePath);
                }

            }

            public string ContentText {
                get {
                    Encoding encoding = (CharSet == null)? Encoding.UTF8 : Encoding.GetEncoding(CharSet);
                    return encoding.GetString(Content);
                }
            }

            public string FileExtension => CachedData.Extension;

            public Stream CreateReadStream() {
                return new MemoryStream(Content);
            }

            ~CacheableHttpRequest() {

            }




        }


    }
}

