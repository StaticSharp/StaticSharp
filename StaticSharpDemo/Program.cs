using StaticSharpWeb;
using StaticSharpDemo.Content;
using StaticSharpEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface IStaticGenerator {
        Uri BaseUri { get; }
        string BaseDirectory { get; }
        string TempDirectory { get; }
    }

    public abstract class StaticGenerator : IStaticGenerator {
        public Uri BaseUri { get; set; }

        public string BaseDirectory { get; set; }

        public string TempDirectory { get; set; }

        public abstract IEnumerable<INode> GetRoots();

        public abstract Uri GetNodeUri(INode node);

        //public string NodeToUrl()

        public IEnumerable<IPage> Pages 
            => GetRoots().SelectMany(x => GetPages(x));

        private IEnumerable<IPage> GetPages(INode node) {
            IEnumerable<IPage> result = node.Representative is IPage page ? Enumerable.Repeat(page, 1) : Enumerable.Empty<IPage>();
            foreach (var i in node.Children) {
                result = result.Concat(GetPages(i));
            }
            return result;
        }
    }
}

namespace StaticSharpDemo {

    public enum Language {
        En,
        Ru
    }

    public class Server : StaticSharpWeb.Server {

        static Server() {
            //CsmlWeb.Storage.StorageDirectory = @"D:\Csml2Cache\";
        }

        public override Uri BaseUri => new("http://localhost/");

        private IStorage _Storage;

        public override IStorage Storage {
            get {
                if (_Storage is null) {
                    Directory.CreateDirectory(TempDirectory);
                    Directory.CreateDirectory(IntermidiateCache);
                    _Storage = new Storage(TempDirectory, IntermidiateCache);
                }
                return _Storage;
            }
        }

        public override string BaseDirectory => throw new NotImplementedException();

        public override string TempDirectory => @"D:\Csml2Cache\";

        public string IntermidiateCache => Path.Combine(TempDirectory, "IntermediateCache");

        public override IPage FindPage(string requestPath) {
            if (requestPath == null) {
                return null;
            }
            string[] path = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (path.Length == 0) {
                path = new[] { "index_en.html" };
            }

            var htmlName = path.Last();
            htmlName = htmlName[..htmlName.LastIndexOf('.')].ToLower();

            var lastIndexOf_ = htmlName.LastIndexOf('_');
            Language language = default;

            if (lastIndexOf_ != -1) {
                var languagePart = htmlName[(lastIndexOf_ + 1)..];
                language = Enum.GetValues<Language>().FirstOrDefault(i => htmlName.EndsWith(i.ToString().ToLower()));
                htmlName = htmlName[..lastIndexOf_];
            }
            path[^1] = htmlName;

            //Debug en
            INode result = new CsmlRoot(language);
            var pathList = new List<string[]>();
            var root = new CsmlRoot(language).Children.FirstOrDefault().Name;
            foreach (var pathPart in path) {
                result = result.Children.FirstOrDefault(x => x.Name.ToLower() == pathPart.ToLower());
                if (result == null) return null;
            }
            return result.Representative as IPage;
        }

        public override Uri? ObjectToUri(object obj) {
            return obj is IRepresentative representative && representative.Node is ProtoNode protoNode
                ? new Uri(BaseUri, string.Join('/', representative.Node.Path) + "_" + protoNode.Language.ToString() + ".html")
                : null;
        }
    }

    internal class StaticGenerator : StaticSharpWeb.StaticGenerator, IUrls {

        public StaticGenerator(Uri baseUri, Storage storage)
            => (BaseUri, _context) = (baseUri, new(storage, this));

        private Context _context;

        public override Uri GetNodeUri(INode node) {
            throw new NotImplementedException();
        }

        public override IEnumerable<INode> GetRoots() {
            var language = Enum.GetValues(typeof(Language)).Cast<Language>();
            return language.Select(x => new CsmlRoot(x));
        }

        private static async Task WritePage(IPage page, Context context, string path) {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            await File.WriteAllTextAsync(path, await page.GenerateHtmlAsync(context));
        }

        public async Task GenerateAsync() {
            var tasks = new List<Task>();
            foreach (var page in Pages) {
                var url = ObjectToUri(page);
                var relativeUrl = _context.Urls.BaseUri.MakeRelativeUri(url);
                var path = Path.Combine(@"D:\TestSite", relativeUrl.ToString());
                tasks.Add(WritePage(page, _context, path));
            }
            await Task.WhenAll(tasks);
        }

        public Uri ObjectToUri(object obj) {
            return obj is IRepresentative representative && representative.Node is ProtoNode protoNode
                ? new Uri(BaseUri, string.Join('/', representative.Node.Path) + "_" + protoNode.Language.ToString() + ".html")
                : null;
        }
    }

    //public class ProtoNode : ProtoNode
    internal class Program {

        private static void OpenUrl(string url) {
            try {
                Process.Start(url);
            } catch {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                    Process.Start("xdg-open", url);
                } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                    Process.Start("open", url);
                } else {
                    throw;
                }
            }
        }

        private static void Main(string[] args) {
            // var generator = new StaticSharpDemo.Content.StaticGenerator();
            // var pages = generator.GetPages();


        private static async Task Main(string[] args) {
            var generator =
                new StaticGenerator(new Uri(@"D:/TestSite/"), new Storage(@"D:\TestSite", @"D:\TestSite\IntermediateCache"));
            await generator.GenerateAsync();

            new Server().Run();
        }
    }
}