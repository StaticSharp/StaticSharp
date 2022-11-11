

using StaticSharp.Gears;
using StaticSharp.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp;


public class Generator<NodeType> where NodeType : ProtoNode<NodeType> {
    public INodeToPath NodeToPath { get; }
    public AbsoluteUrl BaseUrl { get; }
    public FilePath BaseDirectory { get; }

    public Generator(INodeToPath nodeToPath, AbsoluteUrl baseUrl, FilePath baseDirectory) {
        NodeToPath = nodeToPath;
        BaseUrl = baseUrl;
        BaseDirectory = baseDirectory;
    }

    public AbsoluteUrl NodeToAbsoluteUrl(Node node) {
        return BaseUrl + NodeToPath.NodeToRelativeUrl(node);
    }

    public FilePath NodeToFilePath(Node node) {
        return BaseDirectory + NodeToPath.NodeToRelativeFilePath(node);
    }
    public FilePath NodeToRedirectFilePath(Node node) {
        return BaseDirectory + NodeToPath.NodeToRelativeDirectory(node) + "index.html";
    }


    protected IEnumerable<NodeType> GetAllNodes(NodeType root) {
        yield return root;
        foreach (var node in root.Children) {
            foreach (var i in GetAllNodes(node))
                yield return i;
        }
    }

    protected Context CreateContext(Node node, Assets assets) {
        return new Context(node, assets, NodeToPath, BaseUrl, null);
    }

    protected async Task GetnerateAndSave(Node node, Context context) {
        var page = node.Representative;
        if (page == null) return;
        var html = await page.GeneratePageHtmlAsync(context);

        var path = NodeToFilePath(node);
        var directory = path.WithoutLast;
        Directory.CreateDirectory(directory.OsPath);

        File.WriteAllText(path.OsPath, html);
    }

    public virtual async Task GenerateAsync(NodeType root) {
        var nodes = GetAllNodes(root);
        var assets = new Assets();
        await Task.WhenAll(nodes.Select(node => GetnerateAndSave(node, CreateContext(node, assets))));
        await assets.StoreAsync(BaseDirectory+"Assets");
    }

    protected void SaveSitemap(string saveSitemap) {
        File.WriteAllText((BaseDirectory + "sitemap.xml").OsPath, saveSitemap);
    }



    protected void ClearBaseDirectory() {
        string doNotDeleteFileName = "doNotDelete.txt";
        var doNotDeleteFilePath = BaseDirectory + doNotDeleteFileName;
        DirectoryInfo directory = new DirectoryInfo(BaseDirectory.OsPath);

        var doNotDelete = new List<string>();
        if (File.Exists(doNotDeleteFilePath.OsPath)) {
            doNotDelete = File.ReadAllLines(doNotDeleteFilePath.OsPath).Where(x=>!string.IsNullOrWhiteSpace(x)).ToList();
            doNotDelete.Add(doNotDeleteFileName);
        }

        foreach (FileInfo file in directory.GetFiles()) {
            var fileName = Path.GetFileName(file.FullName);
            if (!doNotDelete.Contains(fileName)) {
                file.Delete();
            }            
        }
        foreach (DirectoryInfo subDirectory in directory.GetDirectories()) {
            var subDirectoryName = Path.GetFileName(subDirectory.FullName);
            if (subDirectoryName.StartsWith(".")) //.git .svn
                continue;
            if (doNotDelete.Contains(subDirectoryName))
                continue;
            subDirectory.Delete(true);
            
        }


    }

}


/*public static class MultilanguageProtoNodeStatic{
    public static Task GenerateSite<LanguageEnum>(this MultilanguageProtoNode<LanguageEnum> _this) where LanguageEnum : struct, Enum {
        var generator = new MultilanguageStaticGenerator<LanguageEnum>();
        return generator.GenerateAsync(_this);
    }
}*/


public class MultilanguageStaticGenerator<LanguageEnum> : Generator<MultilanguageProtoNode<LanguageEnum>> where LanguageEnum : struct, Enum {

    public MultilanguageStaticGenerator(INodeToPath nodeToPath, AbsoluteUrl baseUrl, FilePath baseDirectory) : base(nodeToPath,baseUrl,baseDirectory) {        
    }

    

    public override async Task GenerateAsync(MultilanguageProtoNode<LanguageEnum> root) {

        ClearBaseDirectory();

        var nodes = GetAllNodes(root);
        var nodesMultilanguage = nodes.SelectMany(x => x.GetAllParallelNodes());

        var assets = new Assets();

        await Task.WhenAll(nodesMultilanguage.Select(node => GetnerateAndSave(node, CreateContext(node, assets))));

        SaveSitemap(CreateSiteMap(root));

        foreach (var node in nodes) {
            GetnerateAndSaveMultilanguageRedirect(node);
        }

        await assets.StoreAsync(BaseDirectory + "Assets");
    }

    public void GetnerateAndSaveMultilanguageRedirect(Node node) {
        if (node.Representative == null)
            return;
        var html = MultilanguageRedirect.GenerateHtml<LanguageEnum>();
        var path = NodeToRedirectFilePath(node);
        File.WriteAllText(path.OsPath, html);
    }

    public string CreateSiteMap(MultilanguageProtoNode<LanguageEnum> root) {
        var nodes = GetAllNodes(root);

        var map = new StringBuilder()
            .AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>")
            .AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" " +
                "xmlns:xhtml=\"http://www.w3.org/1999/xhtml\">");

        foreach (var node in nodes) {
            if (node.Representative == null)
                continue;
            map.AppendLine("\t<url>");
            map.AppendLine($"\t<loc>{NodeToAbsoluteUrl(node)}</loc>");
            foreach (var l in node.GetAllParallelNodes()) {
                var language = l.Language.ToString().ToLower();
                map.AppendLine($"\t\t<xhtml:link rel=\"alternate\" hreflang=\"{language}\" href=\"{NodeToAbsoluteUrl(l)}\"/>");
            }
            map.AppendLine("\t</url>");
        }
        map.AppendLine("</urlset>");
        return map.ToString();
    }


}