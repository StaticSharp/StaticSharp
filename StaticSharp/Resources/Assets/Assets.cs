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
            var hash = await asset.GetContentHashAsync();
            if (!assets.TryGetValue(hash, out var existingAsset)) {
                assets[hash] = asset;
                /*if (existingAsset.ContentHash == asset.ContentHash) {
                    return;
                }*/
            }
            
        }
    }

    public async Task<IAsset?> GetByFilePath(FilePath filePath) {
        foreach (var asset in assets.Values) {
            if (await asset.GetTargetFilePathAsync() == filePath) {
                return asset;
            }
        }
        return null;
    }

    private async Task StoreAssetAsync(IAsset asset, FilePath assetsBaseDirectory) {
        var fullFilePath = (assetsBaseDirectory + await asset.GetTargetFilePathAsync());
        var directory = fullFilePath.WithoutLast.OsPath;
        Directory.CreateDirectory(directory);

        await File.WriteAllBytesAsync(fullFilePath.OsPath, await asset.GetBytesAsync());
    }
    public async Task StoreAsync(FilePath directory) {
        await Task.WhenAll(assets.Values.Select(x=>StoreAssetAsync(x, directory)).ToArray());
    }   


}




