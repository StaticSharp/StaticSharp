using System.Collections;
using System.Collections.Generic;

namespace StaticSharpWeb;

public class ElementContainer : IEnumerable<IElement>, IElementContainer {
    private readonly List<IElement> _items = new();

    IEnumerator IEnumerable.GetEnumerator() {
        return _items.GetEnumerator();
    }

    public IEnumerator<IElement> GetEnumerator() {
        return _items.GetEnumerator();
    }

    public void AddElement(IElement block) {
        _items.Add(block);
    }

}
