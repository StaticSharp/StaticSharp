using StaticSharpEngine;
using StaticSharpWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
/*
namespace StaticSharpDemo.Root {

    public class StaticGenerator : StaticSharpWeb.StaticGenerator {

        public StaticGenerator(Uri baseUri, Storage storage, string outputPath)
            => (BaseUrl, _context, _outputPath) = (baseUri, new(storage, this, new Theme()), outputPath);

        private Context _context;
        private string _outputPath;

        public override string GetLanguage(INode node) {
            if (node is Material material) {
                return material.Language.ToString().ToLower();
            } else {
                return "en";
            }
        }

        public override IEnumerable<dynamic> GetStates()
            => Enum.GetValues(typeof(Language)).Cast<dynamic>();

        private static async Task WritePage(IPage page, Context context, string path) {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            await File.WriteAllTextAsync(path, await page.GeneratePageHtmlAsync(context));
        }

        public async Task GenerateAsync() {
            var tasks = new List<Task>();
            var map = CreateSiteMap();
            foreach (var page in Pages) {
                var representative = page as IRepresentative;
                var url = ProtoNodeToUri(representative?.Node as ProtoNode);
                var relativeUrl = _context.Urls.BaseUrl.MakeRelativeUri(url);
                var path = Path.Combine(_outputPath, relativeUrl.ToString());
                tasks.Add(WritePage(page, _context, path));
            }
            await Task.WhenAll(tasks);
            //await File.WriteAllTextAsync(map);
        }

        public override Uri? ProtoNodeToUri<T>(T? node) where T : class => node is ProtoNode protoNode
            ? new(BaseUrl, string.Join('/', protoNode.Path) + "_" + protoNode.Language.ToString() + ".html")
            : null;
    }
}*/