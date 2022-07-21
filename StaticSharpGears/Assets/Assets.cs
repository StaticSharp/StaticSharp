namespace StaticSharp.Gears;





public class Assets {
    public string Directory { get; init; }
    public Uri BaseUrl { get; init; }



    private static readonly Dictionary<string, IAsset> assets = new();



    public void Add(IAsset asset) {
        if (assets.TryGetValue(asset.Key, out var existingAsset)) {
            if (existingAsset.ContentHash == asset.ContentHash) {
                return;
            }
        }
        assets[asset.Key] = asset;
    }

    public IAsset? GetByFilePath(string filePath) {
        foreach (var asset in assets.Values) {
            if (asset.FilePath == filePath) {
                return asset;
            }
        }
        return null;
    }

    private async Task StoreAssetAsync(IAsset asset) {
        using var fileStream = File.Create(Path.Combine(Directory,asset.FilePath));
        asset.CreateReadStream().CopyTo(fileStream);
        fileStream.Close();
    }
    public async Task StoreAsync() {
        await Task.WhenAll(assets.Values.Select(x=>StoreAssetAsync(x)).ToArray());
    }

    


}




