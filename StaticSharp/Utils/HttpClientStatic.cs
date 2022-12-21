using System.Net.Http;
using System.Net.Http.Headers;

namespace StaticSharp.Gears;

static class HttpClientStatic {

    static HttpClient Create() {
        var result = new HttpClient();
        var productValue = new ProductInfoHeaderValue("StaticSharp", "1.0");
        result.DefaultRequestHeaders.UserAgent.Add(productValue);
        //request.Headers.Add().UserAgent = "StaticSharp";
        return result;

    }
    public static HttpClient Instance { get; private set; } = Create();
}




