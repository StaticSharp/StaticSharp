namespace StaticSharpGears;

static class HttpClientStatic { 
    public static HttpClient Instance { get; private set; } = new HttpClient();
}




