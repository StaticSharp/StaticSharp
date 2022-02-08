using StaticSharpWeb.Html;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public class Footer : IEnumerable<IBlock>, IBlock, IBlockContainer {
        private readonly List<IBlock> _items = new();

        public void AddBlock(IBlock block) {
            _items.Add(block);
        }

        public async Task<INode> GenerateBlockHtmlAsync(Context context) {
            context.Includes.Require(new Style(new AbsolutePath(nameof(Footer) + ".scss")));
            
            var result = new Tag("div", new { Class = "footer" });            
            var items = _items.Select(item => item.GenerateBlockHtmlAsync(context));
            foreach(var item in await Task.WhenAll(items)) {
                result.Add(item);
            }
            result.Add(new JSCall(new AbsolutePath($"{nameof(Footer)}.js")).Generate(context));
            return new Tag("footer") { result };
        }

        public IEnumerator<IBlock> GetEnumerator() {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}