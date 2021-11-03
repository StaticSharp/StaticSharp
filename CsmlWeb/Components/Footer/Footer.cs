using CsmlWeb.Html;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsmlWeb {
    public class Footer : IEnumerable<IBlock>, IBlock, IVerifiedBlockReceiver {
        private readonly List<IBlock> _items = new();

        public void AddBlock(IBlock block) {
            _items.Add(block);
        }

        public async Task<INode> GenerateBlockHtmlAsync(Context context) {
            context.Includes.RequireStyle(new Style(new RelativePath(nameof(Footer) + ".scss")));
            var result = new Tag("div", new { Class = "footer" });            
            var items = _items.Select(item => item.GenerateBlockHtmlAsync(context));
            foreach(var item in await Task.WhenAll(items)) {
                result.Add(item);
            }
            result.Add(new JSCall(new RelativePath($"{nameof(Footer)}.js")).Generate(context));
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