using StaticSharpEngine;
using CsmlWeb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpDemo.Content {





    /*class WebServer : CsmlWeb.Server {

        public WebServer(Uri baseUri, string baseDirectory, string tempDirectory) {
            BaseUri = baseUri;
            BaseDirectory = baseDirectory;
            TempDirectory = tempDirectory;
        }

        public override Uri BaseUri { get; }
        public override string BaseDirectory { get; }
        public override string TempDirectory { get; }

        public override IEnumerable GetStates() {
            throw new NotImplementedException();
        }

        public override IPage ParseUrl(string url) {

            Page page = new Page();
            ;
            if(Path.GetExtension(url).ToLower() == ".html") {

            }

            var language = Language.En;

            var root = new CsmlRoot(language);

            return page;
        }
    }*/

    /*public class HtmlEnvironment : IHtmlEnvironment {
        public IDictionary<string, IResource> Storage => throw new NotImplementedException();

        public IDictionary<string, IResource> Includes => throw new NotImplementedException();

        public IResource GetOrAddStorrableResourceGroup(string key) {
            if(Includes.TryGetValue(key, out var resource)) {
                return resource;
            }
            return null;
        }
    }*/


    class StaticGenerator : CsmlWeb.StaticGenerator {
        public override Uri BaseUri => throw new NotImplementedException();

        public override string BaseDirectory => throw new NotImplementedException();

        public override string TempDirectory => throw new NotImplementedException();

        public override Uri GetNodeUri(INode node, Uri baseUri) {
            throw new NotImplementedException();
        }

        public override IEnumerable<INode> GetRoots() {
            throw new NotImplementedException();
        }
    }




    public partial class ProtoNode {
        public Language Language { get; init; }

        protected T SelectRepresentative<T>(IEnumerable<T> representatives) {
            foreach(var r in representatives) {
                if(r.GetType().Name == Enum.GetName(Language))
                    return r;
            }
            var fallback = representatives.FirstOrDefault(x => x.GetType().Name == "En");
            if(fallback != null) return fallback;
            return representatives.FirstOrDefault();
        }
    }




    abstract partial class ProtoNode : StaticSharpEngine.INode {

        public ProtoNode(Language language) {
            Language = language;
        }

        public abstract ProtoNode Parent { get; }
        INode INode.Parent => Parent;
        public abstract ProtoNode Root { get; }
        INode INode.Root => Root;

        IRepresentative INode.Representative => Representative as IRepresentative;
        public abstract object Representative { get; }

        public abstract IEnumerable<ProtoNode> Children { get; }
        IEnumerable<INode> INode.Children => Children;

        public abstract string Name { get; }


        public abstract string[] Path { get; }        

        public abstract ProtoNode WithLanguage(global::StaticSharpDemo.Language language);
    }




}
