using MimeTypes;
using StaticSharp.Gears;
using System.Text;

namespace StaticSharp {


    public record HttpRequestGenome(HttpRequestMessage HttpRequestMessage) : AssetGenome<HttpRequestGenome, HttpRequestAsset> {
        public HttpRequestGenome(string uri) : this(new Uri(uri)) { }
        public HttpRequestGenome(Uri uri) : this(new HttpRequestMessage(HttpMethod.Get, uri) {

        }) { }

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



        public class HttpRequestAsset : Cacheable<HttpRequestGenome, HttpRequestAsset.Data>, IAsset {

            public class Data {
                public string? CharSet;
                public string MediaType = null!;
                public string ContentHash = null!;
                public string Extension = null!;
            };


            public static readonly string DefaultMediaType = "application/octet-stream";

            public override string? CharSet => CachedData.CharSet;
            public string MediaType => CachedData.MediaType!=null ? CachedData.MediaType : DefaultMediaType;
            
            public string ContentHash => CachedData.ContentHash;
            public string FileExtension => CachedData.Extension;

            protected override void SetGenome(HttpRequestGenome arguments) {
                base.SetGenome(arguments);

                var uri = Genome.HttpRequestMessage.RequestUri;
                if (uri == null) {
                    throw new ArgumentException("HttpRequestMessage.RequestUri");
                }
            }



            protected override async Task CreateAsync() {


                if (!LoadData()) {
                    CachedData = new();

                    var httpResponseMessage = await HttpClientStatic.Instance.SendAsync(Genome.HttpRequestMessage);
                    if (!httpResponseMessage.IsSuccessStatusCode) {
                        throw new Exception(); //TODO: details
                    }

                    CachedData.CharSet = (httpResponseMessage.Content.Headers.ContentType?.CharSet);

                    var mediaType = httpResponseMessage.Content.Headers.ContentType?.MediaType;
                    var path = Genome.HttpRequestMessage.RequestUri?.AbsolutePath;
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

            
            
        }

    }
}

