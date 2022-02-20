using System.Net.Http.Headers;
using System.Text;

namespace StaticSharpGears;



//Instead of inheritance from CacheableHttpRequest, it is better to use aggregation
public class CacheableHttpRequest : Cacheable<CacheableHttpRequest.Constructor, CacheableHttpRequest.Data>, IAsset, IFile {

    public record Constructor(HttpRequestMessage HttpRequestMessage) : Constructor<CacheableHttpRequest> {

        public Constructor(string uri) : this(new Uri(uri)) { }
        public Constructor(Uri uri) : this(new HttpRequestMessage(HttpMethod.Get, uri) {

        }) { }
        protected override CacheableHttpRequest Create() {
            return new CacheableHttpRequest(this);
        }
    }

    public class Data {
        public string? CharSet;
        public string? MediaType;
    };


    public static readonly string DefaultMediaType = "application/octet-stream";

    public SecondaryTask<string?> CharSet { get; init; } = new();
    public SecondaryTask<string> MediaType { get; init; } = new();
    public SecondaryTask<Func<Stream>> Content { get; init; } = new();

    IAwaitable<Func<Stream>> IFile.Content => Content;
    IAwaitable<string> IFile.MediaType => MediaType;
    IAwaitable<string?> IFile.CharSet => CharSet;

    /*private TaskCompletionSource<MediaTypeHeaderValue?> ContentType_TaskCompletionSource = new();
    public Task<MediaTypeHeaderValue?> ContentType => ContentType_TaskCompletionSource.Task;

    private TaskCompletionSource<Func<Stream>> Content_TaskCompletionSource = new();
    public Task<Func<Stream>> Content => Content_TaskCompletionSource.Task;*/

    private string ContentFilePath => Path.Combine(CacheSubDirectory, "content");

    

    protected CacheableHttpRequest(Constructor arguments): base(arguments) {

        var uri = Arguments.HttpRequestMessage.RequestUri;
        if (uri == null) { 
            throw new ArgumentException("HttpRequestMessage.RequestUri");
        }
        //var fileExtension = Path.GetExtension(uri.AbsolutePath);       

        //ContentFilePath = Path.Combine(CacheSubDirectory, "content");

    }


    protected override async Task CreateAsync() {

        void CompleteHeaderTasks() {
            MediaType.SetResult(
                string.IsNullOrEmpty(CachedData?.MediaType)
                ? DefaultMediaType
                : CachedData.MediaType);

            CharSet.SetResult(CachedData?.CharSet);
        }

        void CompleteContentTasks() {
            Content.SetResult(() => File.OpenRead(ContentFilePath));
        }


        if (!LoadData()) {
            CachedData = new();

            var httpResponseMessage = await HttpClientStatic.Instance.SendAsync(Arguments.HttpRequestMessage);
            if (!httpResponseMessage.IsSuccessStatusCode) {
                throw new Exception(); //TODO: details
            }

            CachedData.CharSet = (httpResponseMessage.Content.Headers.ContentType?.CharSet);
            CachedData.MediaType = (httpResponseMessage.Content.Headers.ContentType?.MediaType);

            CompleteHeaderTasks();

            CreateCacheSubDirectory();

            var fileStream = File.OpenWrite(ContentFilePath);
            await httpResponseMessage.Content.CopyToAsync(fileStream);
            fileStream.Close();

            CompleteContentTasks();

            StoreData();
        } else {
            CompleteHeaderTasks();
            CompleteContentTasks();
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



    public async Task<string> ReadAllTextAsync() {        
        var charSet = (await CharSet);
        Encoding encoding;

        if (charSet == null) {
            encoding = Encoding.UTF8;
        } else {
            encoding = Encoding.GetEncoding(charSet);
        }

        var stream = (await Content)();
        var streamReader = new StreamReader(stream, encoding);

        return streamReader.ReadToEnd();
    }


    public Task StoreAsync(string storageRootDirectory) {
        throw new NotImplementedException();
    }

    ~CacheableHttpRequest() {

    }




}




