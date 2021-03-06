using System.Collections.Generic;
namespace StaticSharpWeb;
public interface IContainerConstraints<T0> : IElement { }
public interface IContainerConstraints<T0,T1> : IElement { }
public interface IContainerConstraints<T0,T1,T2> : IElement { }
public interface IContainerConstraints<T0,T1,T2,T3> : IElement { }
public interface IContainerConstraints<T0,T1,T2,T3,T4> : IElement { }
public interface IContainerConstraints<T0,T1,T2,T3,T4,T5> : IElement { }
public interface IContainerConstraints<T0,T1,T2,T3,T4,T5,T6> : IElement { }
public interface IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7> : IElement { }
public interface IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7,T8> : IElement { }
public interface IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> : IElement { }

public static partial class ContainerStatic {
    public static void Add<Collection, T0>(this Collection collection, IContainerConstraints<T0> item)
        where Collection : IElementContainer, T0
    => collection.AddElement(item);
    public static void Add<Collection, T0>(this Collection collection, IEnumerable<IContainerConstraints<T0>> items)
        where Collection : IElementContainer, T0{
        foreach(var i in items){
            collection.Add(i);
        }
    }
    public static void Add<Collection, T0,T1>(this Collection collection, IContainerConstraints<T0,T1> item)
        where Collection : IElementContainer, T0,T1
    => collection.AddElement(item);
    public static void Add<Collection, T0,T1>(this Collection collection, IEnumerable<IContainerConstraints<T0,T1>> items)
        where Collection : IElementContainer, T0,T1{
        foreach(var i in items){
            collection.Add(i);
        }
    }
    public static void Add<Collection, T0,T1,T2>(this Collection collection, IContainerConstraints<T0,T1,T2> item)
        where Collection : IElementContainer, T0,T1,T2
    => collection.AddElement(item);
    public static void Add<Collection, T0,T1,T2>(this Collection collection, IEnumerable<IContainerConstraints<T0,T1,T2>> items)
        where Collection : IElementContainer, T0,T1,T2{
        foreach(var i in items){
            collection.Add(i);
        }
    }
    public static void Add<Collection, T0,T1,T2,T3>(this Collection collection, IContainerConstraints<T0,T1,T2,T3> item)
        where Collection : IElementContainer, T0,T1,T2,T3
    => collection.AddElement(item);
    public static void Add<Collection, T0,T1,T2,T3>(this Collection collection, IEnumerable<IContainerConstraints<T0,T1,T2,T3>> items)
        where Collection : IElementContainer, T0,T1,T2,T3{
        foreach(var i in items){
            collection.Add(i);
        }
    }
    public static void Add<Collection, T0,T1,T2,T3,T4>(this Collection collection, IContainerConstraints<T0,T1,T2,T3,T4> item)
        where Collection : IElementContainer, T0,T1,T2,T3,T4
    => collection.AddElement(item);
    public static void Add<Collection, T0,T1,T2,T3,T4>(this Collection collection, IEnumerable<IContainerConstraints<T0,T1,T2,T3,T4>> items)
        where Collection : IElementContainer, T0,T1,T2,T3,T4{
        foreach(var i in items){
            collection.Add(i);
        }
    }
    public static void Add<Collection, T0,T1,T2,T3,T4,T5>(this Collection collection, IContainerConstraints<T0,T1,T2,T3,T4,T5> item)
        where Collection : IElementContainer, T0,T1,T2,T3,T4,T5
    => collection.AddElement(item);
    public static void Add<Collection, T0,T1,T2,T3,T4,T5>(this Collection collection, IEnumerable<IContainerConstraints<T0,T1,T2,T3,T4,T5>> items)
        where Collection : IElementContainer, T0,T1,T2,T3,T4,T5{
        foreach(var i in items){
            collection.Add(i);
        }
    }
    public static void Add<Collection, T0,T1,T2,T3,T4,T5,T6>(this Collection collection, IContainerConstraints<T0,T1,T2,T3,T4,T5,T6> item)
        where Collection : IElementContainer, T0,T1,T2,T3,T4,T5,T6
    => collection.AddElement(item);
    public static void Add<Collection, T0,T1,T2,T3,T4,T5,T6>(this Collection collection, IEnumerable<IContainerConstraints<T0,T1,T2,T3,T4,T5,T6>> items)
        where Collection : IElementContainer, T0,T1,T2,T3,T4,T5,T6{
        foreach(var i in items){
            collection.Add(i);
        }
    }
    public static void Add<Collection, T0,T1,T2,T3,T4,T5,T6,T7>(this Collection collection, IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7> item)
        where Collection : IElementContainer, T0,T1,T2,T3,T4,T5,T6,T7
    => collection.AddElement(item);
    public static void Add<Collection, T0,T1,T2,T3,T4,T5,T6,T7>(this Collection collection, IEnumerable<IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7>> items)
        where Collection : IElementContainer, T0,T1,T2,T3,T4,T5,T6,T7{
        foreach(var i in items){
            collection.Add(i);
        }
    }
    public static void Add<Collection, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Collection collection, IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7,T8> item)
        where Collection : IElementContainer, T0,T1,T2,T3,T4,T5,T6,T7,T8
    => collection.AddElement(item);
    public static void Add<Collection, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Collection collection, IEnumerable<IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7,T8>> items)
        where Collection : IElementContainer, T0,T1,T2,T3,T4,T5,T6,T7,T8{
        foreach(var i in items){
            collection.Add(i);
        }
    }
    public static void Add<Collection, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Collection collection, IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> item)
        where Collection : IElementContainer, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9
    => collection.AddElement(item);
    public static void Add<Collection, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Collection collection, IEnumerable<IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>> items)
        where Collection : IElementContainer, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9{
        foreach(var i in items){
            collection.Add(i);
        }
    }
}
