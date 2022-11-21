

using StaticSharp.Gears;
using StaticSharp.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        return new Context(NodeToPath.NodeToRelativeDirectory(node), assets, NodeToPath, BaseUrl, null);
    }

    protected async Task GetnerateAndSave(Node node, Context context) {
        var page = node.Representative;
        if (page == null) return;
        var s = Stopwatch.StartNew();
        var html = await page.GeneratePageHtmlAsync(context);
        Console.WriteLine(s.ElapsedMilliseconds);

        var path = NodeToFilePath(node);
        var directory = path.WithoutLast;
        Directory.CreateDirectory(directory.OsPath);

        File.WriteAllText(path.OsPath, html);
    }

    public virtual async Task GenerateAsync(NodeType root) {
        var nodes = GetAllNodes(root);
        var assets = new Assets();

        foreach (var i in nodes) {
            await GetnerateAndSave(i, CreateContext(i, assets));
        }

        //await Task.WhenAll(nodes.Select(node => GetnerateAndSave(node, CreateContext(node, assets))));
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
