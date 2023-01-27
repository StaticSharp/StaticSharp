using Microsoft.AspNetCore.Http;
using System;

namespace StaticSharp {
    public static partial class HttpRequestStatic {
        public static Uri GetBaseUri(this HttpRequest request) {
            return new Uri($"{request.Scheme}://{request.Host}");
        }
    }
}
