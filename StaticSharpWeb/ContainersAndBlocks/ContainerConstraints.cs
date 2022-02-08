namespace StaticSharpWeb;
public interface IContainerConstraints<T0> : IBlock { }
public interface IContainerConstraints<T0,T1> : IBlock { }
public interface IContainerConstraints<T0,T1,T2> : IBlock { }
public interface IContainerConstraints<T0,T1,T2,T3> : IBlock { }
public interface IContainerConstraints<T0,T1,T2,T3,T4> : IBlock { }
public interface IContainerConstraints<T0,T1,T2,T3,T4,T5> : IBlock { }
public interface IContainerConstraints<T0,T1,T2,T3,T4,T5,T6> : IBlock { }
public interface IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7> : IBlock { }
public interface IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7,T8> : IBlock { }
public interface IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> : IBlock { }

public static class ContainerStatic {
    public static void Add<Collection, T0>(this Collection collection, IContainerConstraints<T0> item)
        where Collection : IBlockContainer, T0
    => collection.AddBlock(item);
    public static void Add<Collection, T0,T1>(this Collection collection, IContainerConstraints<T0,T1> item)
        where Collection : IBlockContainer, T0,T1
    => collection.AddBlock(item);
    public static void Add<Collection, T0,T1,T2>(this Collection collection, IContainerConstraints<T0,T1,T2> item)
        where Collection : IBlockContainer, T0,T1,T2
    => collection.AddBlock(item);
    public static void Add<Collection, T0,T1,T2,T3>(this Collection collection, IContainerConstraints<T0,T1,T2,T3> item)
        where Collection : IBlockContainer, T0,T1,T2,T3
    => collection.AddBlock(item);
    public static void Add<Collection, T0,T1,T2,T3,T4>(this Collection collection, IContainerConstraints<T0,T1,T2,T3,T4> item)
        where Collection : IBlockContainer, T0,T1,T2,T3,T4
    => collection.AddBlock(item);
    public static void Add<Collection, T0,T1,T2,T3,T4,T5>(this Collection collection, IContainerConstraints<T0,T1,T2,T3,T4,T5> item)
        where Collection : IBlockContainer, T0,T1,T2,T3,T4,T5
    => collection.AddBlock(item);
    public static void Add<Collection, T0,T1,T2,T3,T4,T5,T6>(this Collection collection, IContainerConstraints<T0,T1,T2,T3,T4,T5,T6> item)
        where Collection : IBlockContainer, T0,T1,T2,T3,T4,T5,T6
    => collection.AddBlock(item);
    public static void Add<Collection, T0,T1,T2,T3,T4,T5,T6,T7>(this Collection collection, IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7> item)
        where Collection : IBlockContainer, T0,T1,T2,T3,T4,T5,T6,T7
    => collection.AddBlock(item);
    public static void Add<Collection, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Collection collection, IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7,T8> item)
        where Collection : IBlockContainer, T0,T1,T2,T3,T4,T5,T6,T7,T8
    => collection.AddBlock(item);
    public static void Add<Collection, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Collection collection, IContainerConstraints<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> item)
        where Collection : IBlockContainer, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9
    => collection.AddBlock(item);
}
