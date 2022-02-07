namespace StaticSharpGears;

public static partial class KeyCalculators {
    public static string GetKey(HttpRequestMessage httpRequestMessage) {
        return KeyUtils.Combine<HttpRequestMessage>(
            httpRequestMessage.RequestUri?.ToString(),
            httpRequestMessage.Method.ToString()
            );
    }
}




