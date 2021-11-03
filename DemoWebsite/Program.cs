using CsmlEngine;
using CsmlWeb;
using CsmlWeb.Resources;
using DemoWebsite.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;


namespace CsmlWeb {

    public interface IResourceGeneratorSettings {
        Uri BaseUri { get; }
        string BaseDirectory { get; }
        string TempDirectory { get; }
    }

    public abstract class AbstractResourceGenerator : IResourceGeneratorSettings {
        public abstract Uri BaseUri { get; }
        public abstract string BaseDirectory { get; }
        public abstract string TempDirectory { get; }

        public void RegisterResource(string hash, IResource resource) {

        }

    }


    public record HtmlContext { }


    public abstract class StaticGenerator : AbstractResourceGenerator {





        public abstract IEnumerable<INode> GetRoots();

        public abstract Uri GetNodeUri(INode node, Uri baseUri);

        //public string NodeToUrl()

        public IEnumerable<IPage> GetPages() {
            return GetRoots().SelectMany(x => GetPages(x));

            /*IEnumerable<IPage> result = Enumerable.Empty<IPage>();
            foreach (var state in GetStates()) {
                var root = new TRoot() as INode;

            }*/
            //return null;
        }

        private IEnumerable<IPage> GetPages(INode node) {
            IEnumerable<IPage> result = node.Representative is IPage page ? Enumerable.Repeat(page, 1) : Enumerable.Empty<IPage>();
            foreach(var i in node.Children) {
                result = result.Concat(GetPages(i));
            }
            return result;
        }
    }



    /*public class HtmlEnvironment : IHtmlEnvironment {
        public IDictionary<string, IResource> Storage => throw new NotImplementedException();

        public IDictionary<string, IResource> Includes => throw new NotImplementedException();

        public IResource GetOrAddStorrableResourceGroup(string key) {
            throw new NotImplementedException();
        }
    }*/



}

namespace DemoWebsite {

    public enum Language {
        En,
        Ru
    }


    public class Server : CsmlWeb.Server {
        static Server() {
            //CsmlWeb.Storage.StorageDirectory = @"D:\Csml2Cache\";
        }
        public override Uri BaseUri => new("http://localhost/");

        private IStorage _Storage;

        public override IStorage Storage {
            get { 
               if (_Storage is null){
                    Directory.CreateDirectory(@"D:\Csml2Cache\");
                    _Storage = new Storage(@"D:\Csml2Cache\");
               }
                return _Storage;
            }   
        }

        public override string BaseDirectory => throw new NotImplementedException();

        public override string TempDirectory => throw new NotImplementedException();

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

        public override Uri ObjectToUri(object obj) {
            return obj is IRepresentative representative && representative.Node is ProtoNode protoNode
                ? new Uri(BaseUri, string.Join('/', representative.Node.Path) + "_" + protoNode.Language.ToString() + ".html")
                : null;
        }

    }


    class Program {

        static void OpenUrl(string url) {
            try {
                Process.Start(url);
            } catch {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                } else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                    Process.Start("xdg-open", url);
                } else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                    Process.Start("open", url);
                } else {
                    throw;
                }
            }
        }

        static void Main(string[] args) {

            //VisualStudio.Open(@"D:\git\csml2\DemoWebsite\Program.cs",60);


            //var localhost = "http://localhost:5000/";
            //OpenUrl($"{localhost}index_ru.html");
            new Server().Run();

        }
    }
}
