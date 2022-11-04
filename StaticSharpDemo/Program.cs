
using StaticSharpDemo.Root;
using StaticSharp.Gears;
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
using StaticSharp.Tree;

namespace StaticSharpWeb {

    public interface IStaticGenerator /*: IUrls*/ {
        Uri BaseUrl { get; }
        string BaseDirectory { get; }
        string TempDirectory { get; }
    }

    /*public abstract class StaticGenerator : IStaticGenerator {
        public Uri BaseUrl { get; set; }

        public string BaseDirectory { get; set; }

        public string TempDirectory { get; set; }

        public IEnumerable<INode> GetRoots()
            => GetStates().Select(x => new αRoot(x));

        public abstract IEnumerable<dynamic> GetStates();

        public abstract Uri? ProtoNodeToUri<T>(T? node) where T : class, INode;

        public virtual string GetLanguage(INode page) => "en";

        public IEnumerable<IPageGenerator> Pages
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

        private IEnumerable<IPageGenerator> GetPages(INode node) {
            IEnumerable<IPageGenerator> result = node.Representative is IPageGenerator page ? Enumerable.Repeat(page, 1) : Enumerable.Empty<IPageGenerator>();
            foreach (var i in node.Children) {
                result = result.Concat(GetPages(i));
            }
            return result;
        }


    }*/
}

namespace StaticSharpDemo {

    

    public class Server  {



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

        //public override string BaseDirectory => throw new NotImplementedException();

        //public override string TempDirectory => AbsolutePath("../../StaticSharpCache");// @"D:\StaticSharpCache\";

        //public string IntermidiateCache => Path.Combine(TempDirectory, "IntermediateCache");

        

        

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

            await new StaticSharp.MultilanguageServer<Language>(
                (language) => new αRoot(language)
                ).RunAsync();
        }
    }
}