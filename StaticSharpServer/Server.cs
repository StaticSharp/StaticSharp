using StaticSharp.Gears;
using StaticSharp.Tree;
using StaticSharp.Utils;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;

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

        protected Context CreateContext(Node node, Uri baseUrl) {
            return new Context(node, Assets, NodeToPath, baseUrl, null, true);
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
            var app = builder.Build();

            foreach (var i in Urls)
                app.Urls.Add(i.ToString());            

            app.MapGet("{**path}", GetAny).WithName("get_any");
            app.MapGet("Assets/{**path}", GetAsset).WithName("get_asset");
            app.MapGet("api/get_page_hash", GetPageHash).WithName("get_page_hash");
            app.MapPut("api/go_to_source_code", GoToSourceCode).WithName("go_to_source_code");

            app.MapGet("api/test", ()=>"re").WithName("test");

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

        private IResult GetPageHash(HttpContext httpContext) {
            var headers = httpContext.Request.GetTypedHeaders();
            if (headers != null) {
                var referer = headers.Referer;
                if (referer != null) {
                    var path = referer.LocalPath;
                    var page = PageFinder.FindPage(path);
                    if (page != null) {
                        return new ResultAsync(async () => {
                            var context = CreateContext(page.VirtualNode, httpContext.Request.GetBaseUri());
                            var html = await page.GeneratePageHtmlAsync(context);
                            var hash = html.ToHashString();
                            return Results.Text(hash);
                        });
                    }
                }
            }
            return Results.BadRequest();
        }

        public IResult GetAny(HttpContext httpContext) {
            var path = httpContext.Request.Path.Value;
            if (path == null) {
                return Results.BadRequest();
            }

            var page = PageFinder.FindPage(path);
            /*if (page == null) {
                page = Get404(request);
            }*/

            if (page == null) {
                return Results.NotFound();
            }
            var context = CreateContext(page.VirtualNode, httpContext.Request.GetBaseUri());

            return new ResultAsync(async () => {
                var html = await page.GeneratePageHtmlAsync(context);
                html = html.Replace("t!dcsctAYNTSYMJaKLcdZPtZ#n@KPIjkK)ppteSZ4t%W)N*3RC8k645V4DUMW5G!", html.ToHashString());
                return new HtmlResult(html);
            });
        }


        public IResult GetAsset(string path, HttpContext httpContext) {

            var asset = Assets.GetByFilePath(path);
            if (asset == null)
                return Results.NotFound();

            var data = asset.ReadAllBites();

            return Results.File(data, asset.MediaType, null, true);
        }


    }
}
