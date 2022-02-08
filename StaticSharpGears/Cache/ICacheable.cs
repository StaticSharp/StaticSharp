namespace StaticSharpGears;

public interface ICacheable {
    void AfterConstruction();
    Task Job { get; }
}


