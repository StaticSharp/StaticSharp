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



       public record HttpRequest(HttpRequestMessage HttpRequestMessage) : Constructor<HttpRequest, CacheableHttpRequest> {
            public HttpRequest(string uri) : this(new Uri(uri)) { }
            public HttpRequest(Uri uri) : this(new HttpRequestMessage(HttpMethod.Get, uri) {

            }) { }
        }

    


        public class CacheableHttpRequest : Cacheable<HttpRequest, CacheableHttpRequest.Data>, IAsset, IFile {



            public class Data {
                public string? CharSet;
                public string? MediaType;
            };


            public static readonly string DefaultMediaType = "application/octet-stream";

            public string? CharSet => CachedData.CharSet;
            public string MediaType => CachedData.MediaType;
            public byte[] Content { get; private set; } = null!;

            /*IAwaitable<Func<Stream>> IFile.Content => Content;
            IAwaitable<string> IFile.MediaType => MediaType;
            IAwaitable<string?> IFile.CharSet => CharSet;*/

            /*private TaskCompletionSource<MediaTypeHeaderValue?> ContentType_TaskCompletionSource = new();
            public Task<MediaTypeHeaderValue?> ContentType => ContentType_TaskCompletionSource.Task;

            private TaskCompletionSource<Func<Stream>> Content_TaskCompletionSource = new();
            public Task<Func<Stream>> Content => Content_TaskCompletionSource.Task;*/

            private string ContentFilePath => Path.Combine(CacheSubDirectory, "content");

            protected override void SetArguments(HttpRequest arguments) {
                base.SetArguments(arguments);

                var uri = Arguments.HttpRequestMessage.RequestUri;
                if (uri == null) {
                    throw new ArgumentException("HttpRequestMessage.RequestUri");
                }
            }



            protected override async Task CreateAsync() {

                /*void CompleteHeaderTasks() {
                    MediaType.SetResult(
                        string.IsNullOrEmpty(CachedData?.MediaType)
                        ? DefaultMediaType
                        : CachedData.MediaType);

                    CharSet.SetResult(CachedData?.CharSet);
                }*/

                /*void CompleteContentTasks() {
                    Content.SetResult(() => File.OpenRead(ContentFilePath));
                }*/


                if (!LoadData()) {
                    CachedData = new();

                    var httpResponseMessage = await HttpClientStatic.Instance.SendAsync(Arguments.HttpRequestMessage);
                    if (!httpResponseMessage.IsSuccessStatusCode) {
                        throw new Exception(); //TODO: details
                    }

                    CachedData.CharSet = (httpResponseMessage.Content.Headers.ContentType?.CharSet);
                    CachedData.MediaType = (httpResponseMessage.Content.Headers.ContentType?.MediaType);

                    //CompleteHeaderTasks();

                    CreateCacheSubDirectory();

                    Content = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                    await File.WriteAllBytesAsync(ContentFilePath, Content);

/*                    var fileStream = File.OpenWrite(ContentFilePath);
                    await httpResponseMessage.Content.CopyToAsync(fileStream);
                    fileStream.Close();

                    CompleteContentTasks();*/

                    StoreData();
                } else {
                    Content = await File.ReadAllBytesAsync(ContentFilePath);

                    /*CompleteHeaderTasks();
                    CompleteContentTasks();*/
                }




                /*try {
                    var httpResponseMessage = await HttpClientStatic.Instance.SendAsync(Arguments.HttpRequestMessage);
                    if (!httpResponseMessage.IsSuccessStatusCode) {
                        var exception = new Exception();
                        throw new Exception(); //TODO: details
                    }
                    var fileStream = File.OpenWrite(ContentFilePath);
                    await httpResponseMessage.Content.CopyToAsync(fileStream);
                    fileStream.Close();
                    //fileStream = File.OpenRead(ContentFilePath);
                    Content_TaskCompletionSource.SetResult(() => File.OpenRead(ContentFilePath));
                    ContentType_TaskCompletionSource.SetResult(httpResponseMessage.Content.Headers.ContentType);
                    CachedData.ContentType = httpResponseMessage.Content.Headers.ContentType;
                }
                catch (Exception e) {
                    ContentType_TaskCompletionSource.TrySetException(e);
                    Content_TaskCompletionSource.TrySetException(e);
                } */
            }



            public string ContentText {
                get {
                    Encoding encoding = (CharSet == null)? Encoding.UTF8 : Encoding.GetEncoding(CharSet);
                    return encoding.GetString(Content);
                }
            }


            public Task StoreAsync(string storageRootDirectory) {
                throw new NotImplementedException();
            }

            ~CacheableHttpRequest() {

            }




        }


    }
}

