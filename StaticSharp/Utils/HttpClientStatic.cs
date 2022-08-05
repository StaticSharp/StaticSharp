using System.Net.Http;

namespace StaticSharp.Gears;

static class HttpClientStatic { 
    public static HttpClient Instance { get; private set; } = new HttpClient();
}




