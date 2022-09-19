namespace StaticSharpEngine {
    public interface ITypedRepresentativeProvider<out T>: INode {
        T Representative { get; }
    }






}
