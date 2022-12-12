using StaticSharp.Gears;
using StaticSharp.Generator;
using StaticSharp.Tree;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp;

public class MultilanguageStaticGenerator<LanguageEnum> : Generator<MultilanguageProtoNode<LanguageEnum>> where LanguageEnum : struct, Enum {

    public MultilanguageStaticGenerator(INodeToPath nodeToPath, AbsoluteUrl baseUrl, FilePath baseDirectory) : base(nodeToPath,baseUrl,baseDirectory) {        
    }    

    public override async Task GenerateAsync(MultilanguageProtoNode<LanguageEnum> root) {

        ClearBaseDirectory();

        var nodes = GetAllNodes(root);
        var nodesMultilanguage = nodes.SelectMany(x => x.GetAllParallelNodes());

        var assets = new Assets();

        foreach (var node in nodesMultilanguage)
            GetnerateAndSave(node, CreateContext(node, assets));

       // await Task.WhenAll(nodesMultilanguage.Select(node =>));

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