using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using StaticSharp.Gears;
using StaticSharp.Tree;
using StaticSharp.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StaticSharp {

    /*public class SimpleServer : Server {
        public override IPageGenerator? FindPage(string requestPath) {
            throw new NotImplementedException();
        }

        public override Uri? NodeToUrl(Uri baseUrl, INode node) {
            throw new NotImplementedException();
        }
    }*/

    public class Server {

        private IPageFinder PageFinder { get; }
        private INodeToPath NodeToPath { get; }


        private Assets Assets { get; init; }



        public Server(IPageFinder pageFinder, INodeToPath nodeToPath) {
            Assets = new Assets();            
            PageFinder = pageFinder;
            NodeToPath = nodeToPath;
        }

        protected virtual IEnumerable<Uri> Urls {
            get {
                yield return new("http://localhost/");
                foreach (var i in GetLocalIPAddresses()) {
                    Console.WriteLine($"http://{i}");
                    yield return new($"http://{i}");
                }
            }
        }

        protected Context CreateContext(Node node, AbsoluteUrl baseUrl) {
            return new Context(NodeToPath.NodeToRelativeDirectory(node), Assets, NodeToPath, baseUrl, null, true);
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


        public Task RunAsync(CancellationToken cancellationToken = default) {

            var builder = WebApplication.CreateBuilder();
            builder.Logging.SetMinimumLevel(LogLevel.Warning);

            var app = builder.Build();



            foreach (var i in Urls)
                app.Urls.Add(i.ToString());            

            app.MapGet("{**path}", GetAny).WithName("get_any");
            app.MapGet("Assets/{**path}", GetAsset).WithName("get_asset");
            app.MapGet("api/get_page_hash", GetPageHash).WithName("get_page_hash");
            app.MapPut("api/go_to_source_code", GoToSourceCode).WithName("go_to_source_code");

            //app.MapGet("api/test", ()=>"re").WithName("test");

            return app.RunAsync(cancellationToken);
        }


        class ShowSourceCodeParameters {
            public string callerFilePath { get; set; } = "";
            public int callerLineNumber { get; set; } = 0;
        }


        private async Task GoToSourceCode(HttpContext httpContext) {
            var parameters = await JsonSerializer.DeserializeAsync<ShowSourceCodeParameters>(httpContext.Request.Body);
            if (parameters == null) {
                return;
            }
            VisualStudio.Open(parameters.callerFilePath, parameters.callerLineNumber);
        }

        public static AbsoluteUrl BaseUrlFromHttpRequest(HttpRequest httpRequest) {
            return new AbsoluteUrl(httpRequest.Scheme, httpRequest.Host.Host);
        }

        private IResult RegirectToClosest(HttpContext httpContext, FilePath closest) {
            return Results.Redirect(new AbsoluteUrl(httpContext.Request.Scheme, httpContext.Request.Host.Host, closest).ToString());
        }

        private IResult GetPageHash(HttpContext httpContext) {
            var headers = httpContext.Request.GetTypedHeaders();
            if (headers != null) {
                var referer = headers.Referer;
                if (referer != null) {
                    
                    var path = referer.LocalPath;
                    var page = PageFinder.FindPage(path, out var closest);
                    if (page != null) {

                        Cache.TrimMutatedItems();

                        var s = Stopwatch.StartNew();
                        var context = CreateContext(page.VirtualNode, BaseUrlFromHttpRequest(httpContext.Request));
                        var html = page.GeneratePageHtml(context);
                        var hash = html.ToHashString();
                        Console.WriteLine(s.ElapsedMilliseconds);

                        return Results.Text(hash);
                    }
                    
                }
            }
            return Results.BadRequest();
        }

        public IResult GetAny(HttpContext httpContext) {
            var path = httpContext.Request.Path.Value;
            Console.WriteLine($"navigation to {httpContext.Request.Path.Value}");


            var extension = Path.GetExtension(path);
            if (!string.IsNullOrEmpty(extension)) {
                if (extension.ToLower() != ".html") {
                    return Results.NotFound();
                }
            }

            if (path == null) {
                return Results.BadRequest();
            }

            var page = PageFinder.FindPage(path, out var closest);
            /*if (page == null) {
                page = Get404(request);
            }*/

            if (page == null) {
                Console.WriteLine($"redirect to {closest}");
                return RegirectToClosest(httpContext, closest);
            }

            Cache.TrimMutatedItems();

            var context = CreateContext(page.VirtualNode, BaseUrlFromHttpRequest(httpContext.Request));

            var html = page.GeneratePageHtml(context);

            var hash = html.ToHashString();
            var tempPath = Path.Combine(Cache.RootDirectory, hash + ".html");
            if (!File.Exists(tempPath)) {
                File.WriteAllText(tempPath, html);
            }                

            html = html.Replace("t!dcsctAYNTSYMJaKLcdZPtZ#n@KPIjkK)ppteSZ4t%W)N*3RC8k645V4DUMW5G!", hash);
            return new HtmlResult(html);

        }


        public IResult GetAsset(string path, HttpContext httpContext) {
            var asset = Assets.GetByFilePath(FilePath.FromOsPath(path));
            if (asset == null)
                return Results.NotFound();

            var mediaType = asset.GetMediaType();

            if (asset is IAssetAsyncData assetAsyncData) {
                return new ResultAsync(async () => {
                    var data = await assetAsyncData.GetDataAsync();
                    return Results.File(data, mediaType, null, true);
                });
            } else {
                return Results.File(asset.Data, mediaType, null, true);
            }           
        }
    }
}
