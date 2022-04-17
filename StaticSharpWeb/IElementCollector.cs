using System.Collections;
using System.Collections.Generic;

namespace StaticSharp;
public interface IVoidEnumerable : IEnumerable {
    IEnumerator IEnumerable.GetEnumerator() {
        return null!;
    }
}


public interface IElementCollector<T> : IVoidEnumerable where T: IElement {
    void AddElement(IElement value);
    //IList<IElement> Children { get; }
}



public static class ElementCollectorStatic {
    public static void Add<T>(this IElementCollector<T> collector, T value) where T : IElement {
        if (value != null)
            collector.AddElement(value);
    }


}
