using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StaticSharpEngine;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface IUrls {
        public Uri BaseUri { get; }

        public Uri? ProtoNodeToUri<T>(T? node) where T: class, INode;
    }

    public abstract class Server : IUrls {
        private const string _pageKey = "pageKey";

        private IWebHost _host = null;

        public abstract IPage? FindPage(string requestPath);

        public abstract  Uri ProtoNodeToUri<T>(T protoNode) where T : class, INode;
        public abstract Uri BaseUri { get; }
        public abstract string BaseDirectory { get; }
        public abstract string TempDirectory { get; }

        public virtual IPage Get404(HttpRequest request) {
            return null;
        }

        public abstract IStorage Storage { get; }

        private async Task<string> GenerateErrorPageAsync(Exception e) {
            var context = new Context(Storage, this);
            return await new ErrorPage(e).GenerateHtmlAsync(context);
        }


        protected virtual async Task HandleHtmlRequestAsync(HttpRequest request, HttpResponse response, RouteData routeData) {
            try {
                var page = FindPage(request.Path.Value);
                if (page == null) {
                    page = Get404(request);
                }

                if (page == null) {
                    response.StatusCode = 404;
                    return;
                }
                var context = new Context(Storage, this);

                var html = await page.GenerateHtmlAsync(context);
                response.Cookies.Append(_pageKey, html.ToHashString());
                await response.WriteAsync(html);
            } catch (Exception e) {
                await response.WriteAsync(await GenerateErrorPageAsync(e));
            }
        }

        protected virtual async Task HandleFileRequestAsync(HttpRequest request, HttpResponse response, RouteData routeData) {
            try {
                var storage = Storage.StorageDirectory;
                var curentDirectory = Environment.CurrentDirectory;
                var path = storage.TrimEnd('\\') + request.Path.Value.Replace("/", @"\");//Path.Combine(storage, request.Path.Value.Replace("/", @"\"));
                //var file = await File.ReadAllBytesAsync(path);
                await response.SendFileAsync(path);
            } catch {
            }
        }

        protected virtual async Task HandleAnyRequestAsync(HttpRequest request, HttpResponse response, RouteData routeData) {
            if (request.Path.Value.ToLower().EndsWith(".html")) {
                await HandleHtmlRequestAsync(request, response, routeData);
            } else if (request.Path == "/") {
                await HandleHtmlRequestAsync(request, response, routeData);
            } else {
                //File
                await HandleFileRequestAsync(request, response, routeData);
            }
        }

        private static async Task<JObject> ParseJsonRequest(HttpRequest request) {
            var streamReader = new StreamReader(request.Body);
            var body = await streamReader.ReadToEndAsync();
            return JObject.Parse(body);
        }

        protected virtual async Task HandleRefreshPageAsync(HttpRequest request, HttpResponse responce, RouteData routeData) {
            var result = await ParseJsonRequest(request);
            var page = FindPage(result["location"]?.ToString());
            if (page == null) { return; }
            var context = new Context(Storage, this);
            var html = await page.GenerateHtmlAsync(context);
            await responce.WriteAsync((html.ToHashString() != result[_pageKey].ToString()).ToString().ToLower());
        }




        protected virtual async Task HandleErrorAsync(HttpRequest request, HttpResponse response, RouteData routeData) {
            var context = request.HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error; // Your exception
            //var code = 500; // Internal Server Error by default
            await response.WriteAsync(await GenerateErrorPageAsync(exception));
        }

        protected virtual async Task FindVisualStudio(HttpRequest request, HttpResponse response, RouteData routeData) {
            var jsonBody = await ParseJsonRequest(request);
            StaticSharpGears.VisualStudio.Open(jsonBody["file"].ToString(), int.Parse(jsonBody["line"].ToString()));
            await response.WriteAsync("true");
        }

        public async Task RunAsync() {
            _host = WebHost.CreateDefaultBuilder().ConfigureServices(x =>
                x.AddLogging(builder =>
                    builder.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        //.AddFilter("NToastNotify", LogLevel.Warning)
                        .AddConsole()))
             .Configure(app => app
                //.UseExceptionHandler("/error")
                .UseExceptionHandler(a => a.Run(async c => {
                    var exceptionHandlerPathFeature = c.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature.Error;
                    await c.Response.WriteAsync(await GenerateErrorPageAsync(exception));
                }))
                .UseRouter(r => r
                    .MapPost("/api/v1/visual_studio", FindVisualStudio)
                    .MapPost("/api/v1/refresh_required", HandleRefreshPageAsync)
                    .MapGet("/{**catchAll}", HandleAnyRequestAsync)
                    .MapGet("/error", HandleErrorAsync))
                )
             .UseUrls(BaseUri.ToString())
             .Build();

            await _host.StartAsync();
            await _host.WaitForShutdownAsync();
        }


    }
}