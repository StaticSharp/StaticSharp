

using StaticSharp.Gears;
using StaticSharp.Resources;
using StaticSharp.Tree;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {

    public struct Context {

        public bool DeveloperMode { get; init; }

        public INodeToPath NodeToPath { get; init; }
        public AbsoluteUrl NodeToAbsoluteUrl(Node node) { 
            return BaseUrl + NodeToPath.NodeToRelativeUrl(node);
        }

        public FilePath NodeToUrlRelativeToCurrentNode(Node node) {
            return PathFromHostToCurrentPage.To(NodeToPath.NodeToRelativeUrl(node));
        }

        //public Node CurrentNode { get; init; }
        public FilePath PathFromHostToCurrentPage { get; init; }
        public AbsoluteUrl BaseUrl { get; init; }

        public FilePath AssetsBaseUrl { get; init; }

        //public Includes Includes { get; init; }

        public IncludesCache<Font, CacheableFont> Fonts { get; } = new();

        public UniqueTagCollection SvgDefs { get; } = new("s");

        public Assets Assets { get; init; }

        private List<KeyValuePair<string, Asset>> Styles { get; } = new();

        private List<KeyValuePair<string, Asset>> Scripts { get; } = new();

        public FontFamily[] FontFamilies { get; set; } = null!;
        public FontFamily[] CodeFontFamilies { get; set; } = null!;
        public FontStyle FontStyle { get; set; } = new();

        public Ref<int> nextIdNumber;
        public string GetUniqueId() {
            var result = $"luid{nextIdNumber}";
            nextIdNumber.Value++;
            return result;
        }


        public async Task<FilePath> AddAssetAsync(Asset asset) {
            await Assets.AddAsync(asset);
            return AssetsBaseUrl + asset.FilePath;
        }

        public void AddScript(Asset asset) {
            if (Scripts.Any(x => x.Key == asset.Key))
                return; 
            Scripts.Add(new KeyValuePair<string, Asset>(asset.Key, asset));
        }

        public Html.Tag GenerateScript() {
            var content = string.Join('\n', Scripts.Select(x => x.Value.ReadAllText()));
            return new Html.Tag("script") {
                new Html.PureHtmlNode(content)
            };
        }

        public void AddStyle(Asset asset) {
            if (Styles.Any(x => x.Key == asset.Key))
                return;
            Styles.Add(new KeyValuePair<string, Asset>(asset.Key, asset));
        }

        public Html.Tag GenerateStyle() {
            var content = string.Join('\n', Styles.Select(x => x.Value.ReadAllText()));
            return new Html.Tag("style") {
                new Html.PureHtmlNode(content)
            };
        }

        public async Task<Html.Tag> GenerateFontsAsync() {
            var fontStyle = new StringBuilder();

            var sortedFonts = Fonts.OrderBy(x => x.Key);

            foreach (var i in sortedFonts.Select(x => x.Value)) {
                fontStyle.AppendLine(await i.GenerateIncludeAsync());
            }
            return new Html.Tag("style") {
                new Html.PureHtmlNode(fontStyle.ToString())
            };
        }



        /*public async Task AddScriptFromResource(string fileName, [CallerFilePath] string callerFilePath = "") {
            var assembly = Assembly.GetCallingAssembly();
            string directory = Path.GetDirectoryName(callerFilePath) ?? "";
            string absoluteFilePath = Path.Combine(directory, fileName);
            var relativeFilePath = AssemblyResourcesUtils.GetFilePathRelativeToProject(assembly, absoluteFilePath);
            var relativeResourcePath = AssemblyResourcesUtils.GetResourcePath(relativeFilePath);

            var script = await (new AssemblyResourceGenome(assembly, relativeResourcePath)).CreateOrGetCached();
            AddScript(script);
        }*/

        public Context(FilePath pathFromHostToCurrentPage, Assets assets, INodeToPath nodeToPath, AbsoluteUrl baseUrl, FilePath? assetsBaseUrl = null, bool developerMode = false) {
            //CurrentNode = currentNode;
            PathFromHostToCurrentPage = pathFromHostToCurrentPage;

            Assets = assets;
            BaseUrl = baseUrl;
            NodeToPath = nodeToPath;
            DeveloperMode = developerMode;

            if (assetsBaseUrl == null)
                AssetsBaseUrl = new("Assets");
            else
                AssetsBaseUrl = assetsBaseUrl.Value;

            //Includes = new Includes();
            nextIdNumber = new(0);
            
        }
        /*public Context(Assets assets, Uri baseUrl, INodeToUrl nodeToUrlConverter, bool developerMode = false) {
            Assets = assets;
            //Urls = urls;
            BaseUrl = baseUrl;
            Includes = new Includes();
            //Parents = Enumerable.Empty<object>();
            NodeToUrlConverter = nodeToUrlConverter;

            nextIdNumber = new(0);
            DeveloperMode = developerMode;
        }*/
    }
}