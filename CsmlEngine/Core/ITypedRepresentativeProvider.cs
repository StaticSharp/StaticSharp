namespace CsmlEngine {
    public interface ITypedRepresentativeProvider<out T> {
        T Representative { get; }
    }

}
