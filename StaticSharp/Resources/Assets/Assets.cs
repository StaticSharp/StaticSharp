using NeoSmart.AsyncLock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp.Gears;


public class Assets {

    private static readonly Dictionary<string, Asset> assets = new();
    public static AsyncLock AsyncLock { get; } = new();

    public async Task AddAsync(Asset asset) {
        using (await Cache.AsyncLock.LockAsync()) {

            if (assets.TryGetValue(asset.Key, out var existingAsset)) {
                if (existingAsset.ContentHash == asset.ContentHash) {
                    return;
                }
            }
            assets[asset.Key] = asset;
        }
    }

    public Asset? GetByFilePath(FilePath filePath) {
        foreach (var asset in assets.Values) {
            if (asset.FilePath == filePath) {
                return asset;
            }
        }
        return null;
    }

    private async Task StoreAssetAsync(Asset asset, FilePath assetsBaseDirectory) {
        var fullFilePath = (assetsBaseDirectory + asset.FilePath);
        var directory = fullFilePath.WithoutLast.OsPath;
        Directory.CreateDirectory(directory);

        await File.WriteAllBytesAsync(fullFilePath.OsPath, asset.ReadAllBites());
    }
    public async Task StoreAsync(FilePath directory) {
        await Task.WhenAll(assets.Values.Select(x=>StoreAssetAsync(x, directory)).ToArray());
    }   


}




