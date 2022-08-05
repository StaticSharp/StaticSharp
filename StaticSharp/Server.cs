using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StaticSharp.Utils;
using StaticSharpEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace StaticSharp {

    public abstract class Server: Gears.INodeToUrl {
        private const string _pageKey = "pageKey";

        private IWebHost _host = null;

        public abstract Gears.IPage? FindPage(string requestPath);

        //public abstract Uri BaseUrl { get; }

        public abstract  Uri? NodeToUrl(Uri baseUrl, INode node);
        public abstract IEnumerable<Uri> Urls { get; }
        public abstract string BaseDirectory { get; }
        public abstract string TempDirectory { get; }

        public Gears.Assets Assets = new Gears.Assets();
        public virtual Gears.IPage Get404(HttpRequest request) {
            return null;
        }

        //public abstract IStorage Storage { get; }

        private Gears.Context CreateContext(HttpRequest request) {
            var baseUrl = new Uri($"{request.Scheme}://{request.Host}") ;
            var context = new Gears.Context(Assets, baseUrl, this);
            return context;
        }

        public static IEnumerable<string> GetLocalIPAddresses() { //todo: move to urils
            var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(x => x.OperationalStatus == OperationalStatus.Up)
                .Where(x => x.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                        || x.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                .Where(x => {
                    var properties = x.GetIPProperties();
                    if (properties == null) return false;
                    return properties.GatewayAddresses.Any();
                });

            foreach (var i in interfaces) {
                var unicastAddresses = i.GetIPProperties().UnicastAddresses;
                var address = unicastAddresses.Where(x => x.Address.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
                if (address != null) {
                    yield return address.Address.ToString();
                }
            }
        }


        private async Task<string> GenerateErrorPageAsync(Gears.Context context, Exception e) {
            return await new Gears.ErrorPage(e).GeneratePageHtmlAsync(context);
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

                

                var html = await page.GeneratePageHtmlAsync(CreateContext(request));
                response.Cookies.Append(_pageKey, html.ToHashString());
                await response.WriteAsync(html);

            } catch (Exception e) {
                await response.WriteAsync(await GenerateErrorPageAsync(CreateContext(request), e));
            }
        }

        protected virtual async Task HandleAssetsRequestAsync(HttpRequest request, HttpResponse response, RouteData routeData) {
            
            try {

                var filePath = routeData.Values.FirstOrDefault().Value?.ToString();
                if (string.IsNullOrEmpty(filePath))
                    return;

                var asset = Assets.GetByFilePath(filePath);
                if (asset == null)
                    return;

                response.Headers.ContentType = asset.MediaType;
                //response.Headers.Add("content-disposition", $"attachment; filename=test");

                //await response.StartAsync();
                var data = asset.ReadAllBites();
                await response.Body.WriteAsync(data, 0, data.Length);

                //await asset.CreateReadStream().CopyToAsync(response.Body);

                //await response.CompleteAsync();

                    /*response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                file.CopyTo(context.Response.OutputStream);*/
                /*var storage = Gears.Assets.Directory;
                var curentDirectory = Environment.CurrentDirectory;
                var path = storage.TrimEnd('\\') + request.Path.Value.Replace("/", @"\");//Path.Combine(storage, request.Path.Value.Replace("/", @"\"));
                //var file = await File.ReadAllBytesAsync(path);
                await response.SendFileAsync(path);*/
            } catch {
            }
        }

        protected virtual async Task HandleAnyRequestAsync(HttpRequest request, HttpResponse response, RouteData routeData) {
            if (request.Path.Value.ToLower().EndsWith(".html")) {
                await HandleHtmlRequestAsync(request, response, routeData);
            } else if (request.Path == "/") {
                await HandleHtmlRequestAsync(request, response, routeData);
            } else {
                //await HandleFileRequestAsync(request, response, routeData);
            }
        }

        private static async Task<JObject> ParseJsonRequest(HttpRequest request) {
            var streamReader = new StreamReader(request.Body);
            var body = await streamReader.ReadToEndAsync();
            return JObject.Parse(body);
        }

        protected virtual async Task HandleRefreshPageAsync(HttpRequest request, HttpResponse responce, RouteData routeData) {
            //TODO: try catch
            var result = await ParseJsonRequest(request);
            var page = FindPage(result["location"]?.ToString());
            if (page == null) { return; }
            var html = await page.GeneratePageHtmlAsync(CreateContext(request));
            await responce.WriteAsync((html.ToHashString() != result[_pageKey].ToString()).ToString().ToLower());
        }




        /*protected virtual async Task HandleErrorAsync(HttpRequest request, HttpResponse response, RouteData routeData) {
            var exceptionHandlerFeature = request.HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionHandlerFeature.Error; // Your exception
            //var code = 500; // Internal Server Error by default
            await response.WriteAsync(await GenerateErrorPageAsync(CreateContext(request), exception));
        }*/

        protected virtual async Task FindVisualStudio(HttpRequest request, HttpResponse response, RouteData routeData) {
            var jsonBody = await ParseJsonRequest(request);
            StaticSharp.Gears.VisualStudio.Open(jsonBody["file"].ToString(), int.Parse(jsonBody["line"].ToString()));
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
                    await c.Response.WriteAsync(await GenerateErrorPageAsync(CreateContext(c.Request), exception));
                }))
                .UseRouter(r => r
                    .MapPost("/api/v1/visual_studio", FindVisualStudio)
                    .MapPost("/api/v1/refresh_required", HandleRefreshPageAsync)
                    .MapGet("/Assets/{**catchAll}", HandleAssetsRequestAsync)
                    .MapGet("/{**catchAll}", HandleAnyRequestAsync)
                    //.MapGet("/error", HandleErrorAsync)
                    )
                )
             .UseUrls(Urls.Select(x=>x.ToString()).ToArray())
             .Build();

            await _host.StartAsync();
            await _host.WaitForShutdownAsync();
        }


    }
}