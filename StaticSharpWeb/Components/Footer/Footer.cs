using StaticSharpWeb.Html;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public class Footer : IEnumerable<IElement>, IElement, IElementContainer {
        private readonly List<IElement> Items = new();

        public void AddElement(IElement block) {
            Items.Add(block);
        }

        public async Task<INode?> GenerateHtmlAsync(Context context) {
            //context.Includes.Require(new Style(AbsolutePath(nameof(Footer) + ".scss")));
            



            /*var result = new Tag("div", new { Class = "footer" });            
            var items = Items.Select(item => item.GenerateHtmlAsync(context));
            foreach(var item in await Task.WhenAll(items)) {
                result.Add(item);
            }*/

            return new Tag("footer") {
                new JSCall(AbsolutePath($"{nameof(Footer)}.js")).Generate(context),
                new Tag("div", new { id = "Content" }),
                await Task.WhenAll(Items.Select(item => item.GenerateHtmlAsync(context)))
        };
        }

        public IEnumerator<IElement> GetEnumerator() {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}