namespace StaticSharp.Tree {
    public interface ITypedRepresentativeProvider<out T>: INode {
        new T Representative { get; }
    }






}
