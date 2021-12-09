using StaticSharpWeb;
using StaticSharpWeb.Html;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb.Components {
    public class NavigationMenu : ISideBarComponent, IEnumerable {

        private List<StaticSharpEngine.INode> _nodes = new();
        private readonly IInline _logoNode;

        public NavigationMenu(IInline logoNode = null, params StaticSharpEngine.INode[] nodes) {
            if (nodes is null) { throw new System.ArgumentNullException(nameof(nodes)); }
            _nodes.AddRange(nodes);
            _logoNode = logoNode;
        }

        public async Task<Tag> GenerateSideBarAsync(Context context) {
            var relativePath = new RelativePath("NavigationMenu.js");
            context.Includes.Require(new Script(relativePath));
            var menuList = new Tag("ul", new { Class = "menu-list" });
            var result = new Tag("aside", new { Class = "menu" }) {
                menuList
            };
            if(_logoNode != null) {
                menuList.Add(await _logoNode.GenerateInlineHtmlAsync(context));
            }
            foreach (var node in _nodes) {
                //result.Add(inline.GetHtml());
                var uri = context.Urls.ProtoNodeToUri(node);
                menuList.Add(new Tag("li") { 
                    new Tag("a", new { href = uri }) {
                        node.Name,
                    }
                });
            }
            result.Add(new JSCall(relativePath).Generate(context));
            context.Includes.Require(new Style(new RelativePath($"{nameof(NavigationMenu)}.scss")));
            return result;
        }

        public void Add(StaticSharpEngine.INode node) => _nodes.Add(node);

        public IEnumerator GetEnumerator() => _nodes.GetEnumerator();
    }
}
