


using NUglify;
using StaticSharp.Gears;

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
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public ConcurrentDictionary<string, FontSubsetBuilder> FontSubsetBuilders { get; } = new(); //Key is Font.Key


        public List<Html.Tag> HeadTags = new();
        public SvgDefs SvgDefs { get; } = new();

        public Assets Assets { get; init; }

        private List<KeyValuePair<string, Genome<IAsset>>> Styles { get; } = new();

        private List<KeyValuePair<string, Genome<IAsset>>> Scripts { get; } = new();

        public FontFamilies FontFamilies { get; set; } = null!;
        public FontFamilyGenome[] CodeFontFamilies { get; set; } = null!;
        //public FontStyle FontStyle { get; set; } = new();

        public FontWeight FontWeight { get; set; } = FontWeight.Regular;
        public bool ItalicFont { get; set; } = false;
        public double LetterSpacing { get; set; } = 0;

        public List<(string temporaryId, string replacement)> TemporaryIdToId { get; } = new();
        

        public Ref<int> nextIdNumber;
        public string CreateId(string? temporaryId = null) {
            var result = "v" + nextIdNumber.Value++;
            if (temporaryId != null) {
                TemporaryIdToId.Add((temporaryId, result));
            }
            return result;
        }

        public string ReplaceTemporaryId(string script) {
            foreach (var i in TemporaryIdToId) {
                script = script.Replace(i.temporaryId, i.replacement);
            }
            return script;
        }


        public FilePath AddAsset(IAsset asset) {
            Assets.Add(asset);
            return AssetsBaseUrl + asset.GetTargetFilePath();
        }

        public void AddScript(Genome<IAsset> genome) {
            if (Scripts.Any(x => x.Key == genome.Key))
                return; 
            Scripts.Add(new KeyValuePair<string, Genome<IAsset>>(genome.Key, genome));
        }

        public Html.Tag GenerateScript() {
            var assets = Scripts.Select(x => x.Value.Result);// await Task.WhenAll(Scripts.Select(x => x.Value.CreateOrGetCached()));
            var content = string.Join('\n', assets.Select(x=>x.Text));

            if (!DeveloperMode) {
                var uglifyResult = Uglify.Js(content);
                content = uglifyResult.Code;
            }

            return new Html.Tag("script") {
                new Html.PureHtmlNode(content)
            };
        }

        public void AddStyle(Genome<IAsset> genome) {
            if (Styles.Any(x => x.Key == genome.Key))
                return;
            Styles.Add(new KeyValuePair<string, Genome<IAsset>>(genome.Key, genome));
        }

        public Html.Tag GenerateStyle() {
            var assets = Styles.Select(x => x.Value.Result);// await Task.WhenAll(Styles.Select(x => x.Value.CreateOrGetCached()));
            var content = string.Join('\n', assets.Select(x => x.Text));

            if (!DeveloperMode) {
                var uglifyResult = Uglify.Css(content);
                content = uglifyResult.Code;
            }

            return new Html.Tag("style") {
                new Html.PureHtmlNode(content)
            };
        }

        public Html.Tag GenerateFontsScript() {
            var body = new Scopes.Group();            

            var fontSubsets = FontSubsetBuilders
                .OrderBy(x => x.Key)
                .Select(x=>x.Value)
                .Select(x=>x.GetFontSubset())
                .ToArray();

            foreach (var i in fontSubsets) {
                var familyName = Javascriptifier.ValueStringifier.Stringify(i.FamilyName);
                var base64 = Javascriptifier.ValueStringifier.Stringify(i.Base64);
                var italic = Javascriptifier.ValueStringifier.Stringify(i.Italic);
                var format = Javascriptifier.ValueStringifier.Stringify(i.Format);
                body.Add($"fonts.push(StaticSharp.LoadFont({familyName},{(int)i.Weight}, {italic}, {format}, {base64}))");
            }


            var script = new Scopes.C.Scope("StaticSharp.InitializeStaticFonts = function ()") {
                "const fonts = []",
                body,
                "return Promise.all(fonts)"
            };

            return new Html.Tag("script") {
                new Html.PureHtmlNode(script.ToString())
            };
        }

        public Html.Tag GenerateFontsStyle() {
            var fontStyle = new StringBuilder();

            var sortedFonts = FontSubsetBuilders.OrderBy(x => x.Key);

            foreach (var i in sortedFonts) {
                fontStyle.AppendLine(i.Value.GenerateInclude());
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
                AssetsBaseUrl = assetsBaseUrl;

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