using System;
using System.IO;
using System.Threading.Tasks;

namespace StaticSharp.Gears;

public abstract record AssetGenome<TFinalGenome, TCacheable> : Genome<TFinalGenome, TCacheable>, IGenome<IAsset>
    where TFinalGenome : AssetGenome<TFinalGenome, TCacheable>
    where TCacheable : ICacheable<TFinalGenome>, IAsset, new() {

    async Task<IAsset> IGenome<IAsset>.CreateAsync() {
        return await base.CreateAsync();
    }
    async Task<IAsset> IGenome<IAsset>.CreateOrGetCached() {
        return await base.CreateOrGetCached();
    }
}
public static class AssetGenome{
    public static IGenome<IAsset> GenomeFromPathOrUrl(string pathOrUrl, string callerFilePath) {
        
        if (File.Exists(pathOrUrl)) {
            return new FileGenome(pathOrUrl);
        }
        var absolutePath = MakeAbsolutePath(pathOrUrl, callerFilePath);
        if (File.Exists(absolutePath)) {
            return new FileGenome(absolutePath);
        }
        if (Uri.TryCreate(pathOrUrl, UriKind.Absolute, out var uri)) {
            return new HttpRequestGenome(uri.ToString());
        }
        //TODO: 
        throw new FileNotFoundException("File or Url not found", pathOrUrl);
    }

}





