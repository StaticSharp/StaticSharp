namespace StaticSharpEngine {
    public interface ITypedRepresentativeProvider<out T> {
        T Representative { get; }
    }

}
