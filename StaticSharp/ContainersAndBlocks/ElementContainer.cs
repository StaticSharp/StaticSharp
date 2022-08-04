using StaticSharpWeb.Html;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb;

public abstract class ElementContainer : IEnumerable, IElementContainer {
    public List<object> Items { get; } = new();

    IEnumerator IEnumerable.GetEnumerator() {
        return Items.GetEnumerator();
    }

    /*public async Task<Tag> Content(Context context) {
        return new Tag(null) {
            await Task.WhenAll(Items.Select(x=>x.GenerateHtmlAsync(context)))
        };
    }*/

    public void AddElement(IElement element) {
        Items.Add(element);
    }

}
