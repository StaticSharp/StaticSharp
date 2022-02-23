namespace StaticSharpWeb;

public interface IContainerConstraintsNone: IElement {}

public static partial class ContainerStatic {
    public static void Add(this IElementContainer collection, IContainerConstraintsNone item) => collection.AddElement(item);
}