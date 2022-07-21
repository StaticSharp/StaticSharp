namespace StaticSharp.Gears;

public interface IAsset : IKeyProvider {
    public Stream CreateReadStream();
    public string MediaType { get; }
    public string ContentHash { get; }
    public string FileExtension { get; }

    public string? CharSet { get; }

    public string FilePath => ContentHash + FileExtension;

}


/*public interface IAsset: IKeyProvider {

    Task StoreAsync(string storageRootDirectory);

}*/




