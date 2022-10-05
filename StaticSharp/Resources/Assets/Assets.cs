using NeoSmart.AsyncLock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp.Gears;


public class Assets {
    public string Directory { get; init; }
    public Uri BaseUrl { get; init; }

    private static readonly Dictionary<string, IAsset> assets = new();
    public static AsyncLock AsyncLock { get; } = new();

    public async Task AddAsync(IAsset asset) {
        using (await Cache.AsyncLock.LockAsync()) {

            if (assets.TryGetValue(asset.Key, out var existingAsset)) {
                if (existingAsset.ContentHash == asset.ContentHash) {
                    return;
                }
            } else {
                Console.WriteLine("!assets.TryGetValue");
            }

            assets[asset.Key] = asset;
        }
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
        var fullFilePath = Path.Combine(Directory, asset.FilePath);
        await File.WriteAllBytesAsync(fullFilePath, asset.ReadAllBites());
    }
    public async Task StoreAsync() {
        await Task.WhenAll(assets.Values.Select(x=>StoreAssetAsync(x)).ToArray());
    }   


}




