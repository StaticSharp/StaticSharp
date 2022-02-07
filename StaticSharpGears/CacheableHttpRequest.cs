using System.Net.Http.Headers;
using System.Text;

namespace StaticSharpGears;


//Instead of inheritance from CacheableHttpRequest, it is better to use aggregation
public class CacheableHttpRequest : Cacheable<CacheableHttpRequest.Constructor, CacheableHttpRequest.Data>, IAsset {

    public record Constructor(HttpRequestMessage HttpRequestMessage) : Constructor<CacheableHttpRequest> {

        public Constructor(string uri) : this(new Uri(uri)) { }
        public Constructor(Uri uri) : this(new HttpRequestMessage(HttpMethod.Get, uri) {

        }) { }
        protected override CacheableHttpRequest Create() {
            return new CacheableHttpRequest(this);
        }
    }

    public class Data {
        public MediaTypeHeaderValue? ContentType;
    };


    private TaskCompletionSource<MediaTypeHeaderValue?> ContentType_TaskCompletionSource = new();
    public Task<MediaTypeHeaderValue?> ContentType => ContentType_TaskCompletionSource.Task;

    private TaskCompletionSource<Func<Stream>> Content_TaskCompletionSource = new();
    public Task<Func<Stream>> Content => Content_TaskCompletionSource.Task;

    private string ContentFilePath => Path.Combine(CacheSubDirectory, "content");

    protected CacheableHttpRequest(Constructor arguments): base(arguments) {

        var uri = Arguments.HttpRequestMessage.RequestUri;
        if (uri == null) { 
            throw new ArgumentNullException("HttpRequestMessage.RequestUri");
        }
        //var fileExtension = Path.GetExtension(uri.AbsolutePath);       

        //ContentFilePath = Path.Combine(CacheSubDirectory, "content");

    }

    protected override void Load() {
        //var fileStream = File.OpenRead(ContentFilePath);
        Content_TaskCompletionSource.SetResult(()=> File.OpenRead(ContentFilePath));
        ContentType_TaskCompletionSource.SetResult(CachedData.ContentType);
    }

    protected override async Task CreateAsync() {

        try {
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
        }        
    }



    public async Task<string> ReadAllTextAsync() {        
        var charSet = (await ContentType)?.CharSet;
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

    /*public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync() {
        await DisposeAsyncCore();
        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            job.Wait();
        }
    }

    protected virtual async ValueTask DisposeAsyncCore() {
        await job;//.ConfigureAwait(false);
    }*/


}




