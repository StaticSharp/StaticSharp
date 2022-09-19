

using StaticSharp.Resources;
using StaticSharpEngine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp.Gears {

    public struct Context { 

        public INodeToUrl NodeToUrlConverter { get; init; }
        public Uri? NodeToUrl(INode node) { 
            return NodeToUrlConverter.NodeToUrl(BaseUrl, node);
        }
        public Uri BaseUrl { get; init; }

        public Uri BaseAssetsUrl => new Uri(BaseUrl, "Assets/");

        public Includes Includes { get; init; }

        public IncludesCache<Font, CacheableFont> Fonts { get; } = new();

        public UniqueTagCollection SvgDefs { get; } = new("s");

        public Assets Assets { get; init; }

        private List<KeyValuePair<string, IAsset>> Scripts { get; } = new();

        public FontFamily[] FontFamilies { get; set; } = null!;
        public FontStyle FontStyle { get; set; } = new();

        public Ref<int> nextIdNumber;
        public string GetUniqueId() {
            var result = $"luid{nextIdNumber}";
            nextIdNumber.Value++;
            return result;
        }


        public Uri AddAsset(IAsset asset) {
            Assets.Add(asset);
            return new Uri(BaseAssetsUrl, asset.FilePath);
        }

        public void AddScript(IAsset asset) {
            if (Scripts.Any(x => x.Key == asset.Key))
                return; 
            Scripts.Add(new KeyValuePair<string, IAsset>(asset.Key, asset));
        }

        public Html.Tag GenerateScript() {
            var scriptCode = string.Join('\n', Scripts.Select(x => x.Value.ReadAllText()));
            return new Html.Tag("script") {
                new Html.PureHtmlNode(scriptCode)
            };
        }

        public async Task AddScriptFromResource(string fileName, [CallerFilePath] string callerFilePath = "") {
            var assembly = Assembly.GetCallingAssembly();
            string directory = Path.GetDirectoryName(callerFilePath) ?? "";
            string absoluteFilePath = Path.Combine(directory, fileName);
            var relativeFilePath = AssemblyResourcesUtils.GetFilePathRelativeToProject(assembly, absoluteFilePath);
            var relativeResourcePath = AssemblyResourcesUtils.GetResourcePath(relativeFilePath);

            var script = await (new AssemblyResourceGenome(assembly, relativeResourcePath)).CreateOrGetCached();
            AddScript(script);
        }


        public Context(Assets assets, Uri baseUrl, INodeToUrl nodeToUrlConverter) {
            Assets = assets;
            //Urls = urls;
            BaseUrl = baseUrl;
            Includes = new Includes();
            //Parents = Enumerable.Empty<object>();
            NodeToUrlConverter = nodeToUrlConverter;

            nextIdNumber = new(0);
        }
    }
}