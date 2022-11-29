using NeoSmart.AsyncLock;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp.Gears;


public class Assets {

    private static readonly ConcurrentDictionary<FilePath, IAsset> assets = new(new FilePathEqualityComparer());
    //public static AsyncLock AsyncLock { get; } = new();

    private void Add(FilePath filePath, IAsset asset) {

    
    }

    public async Task AddAsync(IAsset asset) {
        var path = await asset.GetTargetFilePathAsync();
        assets.GetOrAdd(path, asset);

        /*using (await Cache.AsyncLock.LockAsync()) {
            var hash = await asset.GetContentHashAsync();
            if (!assets.TryGetValue(hash, out var existingAsset)) {
                assets[hash] = asset;
            }
            
        }*/
    }

    public IAsset? GetByFilePath(FilePath filePath) {
        return assets.GetValueOrDefault(filePath);


        /*foreach (var asset in assets.Values) {
            if (await asset.GetTargetFilePathAsync() == filePath) {
                return asset;
            }
        }
        return null;*/
    }

    private async Task StoreAssetAsync(IAsset asset, FilePath fullFilePath) {
        var directory = fullFilePath.WithoutLast.OsPath;
        Directory.CreateDirectory(directory);

        await File.WriteAllBytesAsync(fullFilePath.OsPath, await asset.GetBytesAsync());
    }
    public async Task StoreAsync(FilePath directory) {
        await Task.WhenAll(assets.Select(x=>StoreAssetAsync(x.Value, directory + x.Key)).ToArray());
    }   


}




