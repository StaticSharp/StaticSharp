using MimeTypes;
using StaticSharp.Gears;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StaticSharp {


    public record HttpRequestGenome(HttpRequestMessage HttpRequestMessage) : Genome<Asset> {
        public HttpRequestGenome(string uri) : this(new Uri(uri)) { }
        public HttpRequestGenome(Uri uri) : this(new HttpRequestMessage(HttpMethod.Get, uri) {

        }) {}

        class Data {
            public string? CharSet;
            public string MediaType = null!;
            public string ContentHash = null!;
            public string Extension = null!;
        };

        public override async Task<Asset> CreateAsync() {
            Data data;
            Func<byte[]> contentCreator = null!;
            if (!LoadData(out data)) {

                var httpResponseMessage = await HttpClientStatic.Instance.SendAsync(HttpRequestMessage);
                if (!httpResponseMessage.IsSuccessStatusCode) {
                    throw new Exception($"Failed to get {HttpRequestMessage.RequestUri} with code {httpResponseMessage.StatusCode}");
                    //FIXME:
                    //appears as
                    //One or more errors occurred. (Failed to get https://raw.githubusercontent.com/Templarian/MaterialDesign/master/svg/book-variant-multiple.svg with code NotFound)Failed to get https://raw.githubusercontent.com/Templarian/MaterialDesign/master/svg/book-variant-multiple.svg with code NotFound
                }

                data.CharSet = (httpResponseMessage.Content.Headers.ContentType?.CharSet);

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
                StoreData(data);

            } else {
                contentCreator = ()=> FileUtils.ReadAllBytes(ContentFilePath);
            }

            return new Asset(
                contentCreator,
                data.Extension,
                data.MediaType,
                data.ContentHash,
                data.CharSet
                );
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

