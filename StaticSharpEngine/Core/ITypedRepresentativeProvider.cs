namespace StaticSharpEngine {
    public interface ITypedRepresentativeProvider<out T>: INode {
        T Representative { get; }
    }



    /*public static class TypedRepresentativeProviderStatic {
        public static void Add<Collection, T>(this Collection collection, ITypedRepresentativeProvider<T> item) { 
            collection.Add(item.Representative);
        }
    }*/


}
