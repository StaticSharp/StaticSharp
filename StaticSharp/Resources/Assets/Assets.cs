using NeoSmart.AsyncLock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp.Gears;


public class Assets {

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

    private async Task StoreAssetAsync(IAsset asset, string assetsBaseDirectory) {
        var fullFilePath = Path.Combine(assetsBaseDirectory, asset.FilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath)!);

        await File.WriteAllBytesAsync(fullFilePath, asset.ReadAllBites());
    }
    public async Task StoreAsync(string directory) {
        await Task.WhenAll(assets.Values.Select(x=>StoreAssetAsync(x, directory)).ToArray());
    }   


}




