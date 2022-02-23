using StaticSharpWeb.Html;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public class Footer : IEnumerable<IElement>, IElement, IElementContainer {
        private readonly List<IElement> _items = new();

        public void AddElement(IElement block) {
            _items.Add(block);
        }

        public async Task<INode> GenerateHtmlAsync(Context context) {
            context.Includes.Require(new Style(AbsolutePath(nameof(Footer) + ".scss")));
            
            var result = new Tag("div", new { Class = "footer" });            
            var items = _items.Select(item => item.GenerateHtmlAsync(context));
            foreach(var item in await Task.WhenAll(items)) {
                result.Add(item);
            }
            result.Add(new JSCall(AbsolutePath($"{nameof(Footer)}.js")).Generate(context));
            return new Tag("footer") { result };
        }

        public IEnumerator<IElement> GetEnumerator() {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}