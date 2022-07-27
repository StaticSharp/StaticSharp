namespace StaticSharp.Gears;

public interface ICacheable<in TGenome> {

    void SetGenome(TGenome genome);
    Task CreateAsync();
    //Task Job { get; }
}


