namespace StaticSharpEngine {
    public interface ITypedRepresentativeProvider<out T>: INode {
        new T Representative { get; }
    }






}
