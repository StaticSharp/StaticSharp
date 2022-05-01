namespace StaticSharp.Gears;

public interface ICacheable<in TArguments> {

    void SetArguments(TArguments arguments);
    Task CreateAsync();
    //Task Job { get; }
}


