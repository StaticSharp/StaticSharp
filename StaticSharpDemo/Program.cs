using StaticSharpDemo.Content;
using StaticSharpEngine;
using StaticSharpWeb;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface IStaticGenerator : IUrls {
        Uri BaseUri { get; }
        string BaseDirectory { get; }
        string TempDirectory { get; }
    }

    public abstract class StaticGenerator : IStaticGenerator {
        public Uri BaseUri { get; set; }

        public string BaseDirectory { get; set; }

        public string TempDirectory { get; set; }

        public IEnumerable<INode> GetRoots()
            => GetStates().Select(x => new StaticSharpRoot(x));

        public abstract IEnumerable<dynamic> GetStates();

        public abstract Uri? ProtoNodeToUri<T>(T? node) where T : class, INode;

        public virtual string GetLanguage(INode page) => "en";

        public IEnumerable<IPage> Pages
            => GetRoots().SelectMany(x => GetPages(x));


        public IEnumerable<INode> GetAllNodes(INode root) {
            var result = root.Children;
            foreach (var node in result) {
                result = result.Concat(GetAllNodes(node));
            }
            return result;
        }

        public StringBuilder CreateSiteMap() {
            var map = new StringBuilder()
                .AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>")
                .AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" " +
                    "xmlns:xhtml=\"http://www.w3.org/1999/xhtml\">");
            var roots = GetRoots();
            foreach (var node in roots.SelectMany(root => GetAllNodes(root))) {
                map.AppendLine("\t<url>");
                map.AppendLine($"\t<loc>{ProtoNodeToUri(node)}</loc>");
                foreach (var l in (node as ProtoNode).GetAllParallelNodes()) {
                    map.AppendLine($"\t\t<xhtml:link rel=\"alternate\" hreflang=\"{l.Key}\" href=\"{ProtoNodeToUri(l.Value)}\"/>");
                }

                map.AppendLine("\t</url>");
            }

            map.AppendLine("</urlset>");
            return map;
        }

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

        static Server() { }

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

            INode result = new StaticSharpRoot(language);
            var pathList = new List<string[]>();
            var root = new StaticSharpRoot(language).Children.FirstOrDefault().Name;
            foreach (var pathPart in path) {
                result = result.Children.FirstOrDefault(x => x.Name.ToLower() == pathPart.ToLower());
                if (result == null) return null;
            }
            return result.Representative as IPage;
        }


        public override Uri ProtoNodeToUri<T>(T node) {
            return node is ProtoNode protoNode
                ? new Uri(BaseUri, string.Join('/', protoNode.Path) + "_" + protoNode.Language.ToString() + ".html")
                : null;
        }
    }

    internal class Program {

        private static async Task Main(string[] args) {
            //var generator = new Content.StaticGenerator(
            //        new Uri(@"D:/TestSite/"),
            //        new Storage(@"D:\TestSite", @"D:\TestSite\IntermediateCache"),
            //        @"D:\staticsharp.github.io"
            //);
            //await generator.GenerateAsync();

            await new Server().RunAsync();
        }
    }
}