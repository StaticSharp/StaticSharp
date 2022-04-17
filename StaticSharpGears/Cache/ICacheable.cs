namespace StaticSharp.Gears;

public interface ICacheable {
    void AfterConstruction();
    Task Job { get; }
}


