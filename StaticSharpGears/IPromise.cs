namespace StaticSharp.Gears;

public interface IPromise<T> { 
    public Task<T> GetAsync();
}




