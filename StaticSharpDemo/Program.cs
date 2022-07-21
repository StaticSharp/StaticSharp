using StaticSharp.Gears;
using StaticSharpDemo.Root;
using StaticSharpEngine;
using StaticSharpWeb;
using System;
using System.Collections.Generic;



using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface IStaticGenerator /*: IUrls*/ {
        Uri BaseUrl { get; }
        string BaseDirectory { get; }
        string TempDirectory { get; }
    }

    public abstract class StaticGenerator : IStaticGenerator {
        public Uri BaseUrl { get; set; }

        public string BaseDirectory { get; set; }

        public string TempDirectory { get; set; }

        public IEnumerable<INode> GetRoots()
            => GetStates().Select(x => new αRoot(x));

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

    public class Server : StaticSharp.Server {

        static Server() { }

        public override IEnumerable<Uri> Urls {
            get
            {
                //new Uri[] {
                yield return new("http://localhost/");
                foreach (var i in GetLocalIPAddresses()) {
                    Console.WriteLine($"http://{i}");
                    yield return new($"http://{i}");
                }
                //yield return new(GetLocalIPAddress());
            }
        }

        //delete me
        //public override Uri BaseUrl => new("http://localhost/");
        

        /*private IStorage _Storage;

        public override IStorage Storage {
            get {
                if (_Storage is null) {
                    Directory.CreateDirectory(TempDirectory);
                    Directory.CreateDirectory(IntermidiateCache);
                    _Storage = new Storage(TempDirectory, IntermidiateCache);
                }
                return _Storage;
            }
        }*/

        public override string BaseDirectory => throw new NotImplementedException();

        public override string TempDirectory => AbsolutePath("../../StaticSharpCache");// @"D:\StaticSharpCache\";

        public string IntermidiateCache => Path.Combine(TempDirectory, "IntermediateCache");

        

        public override IPage? FindPage(string requestPath) {
            if (requestPath == null) {
                return null;
            }
            
            string[] path = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            Language language = default;


            if (path.Length == 0) {
                return new αRoot(default).Representative;
            }

            var htmlName = path.Last();
            htmlName = htmlName[..htmlName.LastIndexOf('.')].ToLower();

            var lastIndexOf_ = htmlName.LastIndexOf('_');            

            if (lastIndexOf_ != -1) {
                var languagePart = htmlName[(lastIndexOf_ + 1)..];
                language = Enum.GetValues<Language>().FirstOrDefault(i => htmlName.EndsWith(i.ToString().ToLower()));
                htmlName = htmlName[..lastIndexOf_];
            }
            path[^1] = htmlName;
            
            if (path.Length == 1 && path[0] == "index")
                return new αRoot(language).Representative;

            INode result = new αRoot(language);
            var pathList = new List<string[]>();
            //var root = new αRoot(language).Children.FirstOrDefault().Name;
            foreach (var pathPart in path) {
                result = result.Children.FirstOrDefault(x => x.Name.ToLower() == pathPart.ToLower());
                if (result == null) return null;
            }
            return result.Representative as IPage;
        }


        public override Uri? NodeToUrl(Uri baseUrl, INode node) {
            if (node is ProtoNode protoNode) {
                string path;
                if (protoNode.Path.Length == 0) {//root
                    path = "Index";
                } else {
                    path = string.Join('/', protoNode.Path);
                }
                return new Uri(baseUrl, path + "_" + protoNode.Language.ToString() + ".html");

            } else {
                throw new Exception($"ProtoNodeToUri. {node.GetType()} is not ProtoNode");
            }
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

            StaticSharp.Gears.Cache.Directory = AbsolutePath(".cache");

            await new Server().RunAsync();
        }
    }
}