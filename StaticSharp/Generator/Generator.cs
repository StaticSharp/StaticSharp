

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
    public Uri BaseUrl { get; }
    public string BaseDirectory { get; }

    public Generator(INodeToPath nodeToPath, Uri baseUrl, string baseDirectory) {
        NodeToPath = nodeToPath;
        BaseUrl = baseUrl;
        BaseDirectory = baseDirectory;
    }

    public Uri NodeToAbsoluteUrl(INode node) {
        return new Uri(BaseUrl, NodeToPath.NodeToRelativeUrl(node));
    }

    public string NodeToFilePath(INode node) {
        return Path.GetFullPath(BaseDirectory+ NodeToPath.NodeToRelativeFilePath(node));
    }


    protected IEnumerable<NodeType> GetAllNodes(NodeType root) {
        yield return root;
        foreach (var node in root.Children) {
            foreach (var i in GetAllNodes(node))
                yield return i;
        }
    }

    protected Context CreateContext(Assets assets) {
        return new Context(new ContextOptions(assets, NodeToPath, BaseUrl, new Uri(BaseUrl, "Assets")));
    }

    protected async Task GetnerateAndSave(INode node, Context context) {
        var page = node.Representative;
        if (page == null) return;
        var html = await page.GeneratePageHtmlAsync(context);

        var path = NodeToFilePath(node);
        var directory = Path.GetDirectoryName(path);
        if (directory != null)
            Directory.CreateDirectory(directory);

        File.WriteAllText(path, html);
    }

    public virtual async Task GenerateAsync(NodeType root) {
        var nodes = GetAllNodes(root);
        var assets = new Assets();
        await Task.WhenAll(nodes.Select(node => GetnerateAndSave(node, CreateContext(assets))));
        await assets.StoreAsync(Path.Combine(BaseDirectory,"Assets"));
    }

    protected void SaveSitemap(string saveSitemap) {
        File.WriteAllText(Path.Combine(BaseDirectory, "sitemap.xml"), saveSitemap);
    }



    protected void ClearBaseDirectory() {
        string doNotDeleteFileName = "doNotDelete.txt";
        string doNotDeleteFilePath = Path.Combine(BaseDirectory, doNotDeleteFileName);
        DirectoryInfo directory = new DirectoryInfo(BaseDirectory);

        var doNotDelete = new List<string>();
        if (File.Exists(doNotDeleteFilePath)) {
            doNotDelete = File.ReadAllLines(doNotDeleteFilePath).Where(x=>!string.IsNullOrWhiteSpace(x)).ToList();
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

    public MultilanguageStaticGenerator(INodeToPath nodeToPath, Uri baseUrl, string baseDirectory) : base(nodeToPath,baseUrl,baseDirectory) {        
    }

    

    public override async Task GenerateAsync(MultilanguageProtoNode<LanguageEnum> root) {

        ClearBaseDirectory();

        var nodes = GetAllNodes(root);
        var nodesMultilanguage = nodes.SelectMany(x => x.GetAllParallelNodes());

        var assets = new Assets();

        await Task.WhenAll(nodesMultilanguage.Select(node => GetnerateAndSave(node, CreateContext(assets))));

        SaveSitemap(CreateSiteMap(root));

        await assets.StoreAsync(Path.Combine(BaseDirectory, "Assets"));
    }

    public void GenerateIndex() {
        var html = """
            <!DOCTYPE html>
            <html>
              <head>
                <meta http-equiv="refresh" content="7; url='https://www.w3docs.com'" />
              </head>
              <body>
                <p>Please follow <a href="https://www.w3docs.com">this link</a>.</p>
              </body>
            </html>
            """;
    }

    public string CreateSiteMap(MultilanguageProtoNode<LanguageEnum> root) {
        var nodes = GetAllNodes(root);

        var map = new StringBuilder()
            .AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>")
            .AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" " +
                "xmlns:xhtml=\"http://www.w3.org/1999/xhtml\">");

        foreach (var node in nodes) {
            map.AppendLine("\t<url>");
            map.AppendLine($"\t<loc>{NodeToAbsoluteUrl(node)}</loc>");
            foreach (var l in node.GetAllParallelNodes()) {
                if (l.Representative == null)
                    continue;
                var language = l.Language.ToString().ToLower();
                map.AppendLine($"\t\t<xhtml:link rel=\"alternate\" hreflang=\"{language}\" href=\"{NodeToAbsoluteUrl(l)}\"/>");
            }
            map.AppendLine("\t</url>");
        }
        map.AppendLine("</urlset>");
        return map.ToString();
    }


}