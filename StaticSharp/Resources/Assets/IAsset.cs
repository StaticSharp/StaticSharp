namespace StaticSharp.Gears;

public interface IAsset : IKeyProvider {
    public byte[] ReadAllBites();
    public string ReadAllText();
    public string MediaType { get; }
    public string ContentHash { get; }
    public string FileExtension { get; }
    public string? CharSet { get; }
    public string FilePath => ContentHash + FileExtension;
}




