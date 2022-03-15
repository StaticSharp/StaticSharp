using System.Collections;
using System.Collections.Generic;

namespace StaticSharpWeb;

public abstract class ElementContainer : Item, IEnumerable, IElementContainer {
    public List<IElement> Items { get; } = new();

    IEnumerator IEnumerable.GetEnumerator() {
        return Items.GetEnumerator();
    }

    public void AddElement(IElement element) {
        Items.Add(element);
    }

}
