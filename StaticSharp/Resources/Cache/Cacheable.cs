using System.Text.Json;

using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics.CodeAnalysis;

namespace StaticSharp.Gears;

public abstract class Cacheable<TGenome> : ICacheable<TGenome>, IKeyProvider
    where TGenome : class, IKeyProvider {
    public TGenome Genome { get; private set; } = null!;
    public string Key { get; private set; } = null!;
    //public Task Job { get; protected set; } = null!;

    //public virtual IEnumerable<SecondaryTask>

    protected virtual void SetGenome(TGenome genome) {
        Genome = genome;
        Key = Genome.Key;
    }
    void ICacheable<TGenome>.SetGenome(TGenome genome) => SetGenome(genome);


    protected abstract Task CreateAsync();
    Task ICacheable<TGenome>.CreateAsync() => CreateAsync();






}

public abstract class CacheableToFile<TGenome> : Cacheable<TGenome>
        where TGenome : class, IKeyProvider
    {

    
    //protected string CachedDataJsonFilePath { get; private set; } = null!;
    //protected string KeyHash { get; private set; } = null!;
    //protected string CacheSubDirectory { get; private set; } = null!;
    //protected virtual string ContentFilePath => Path.Combine(CacheSubDirectory, "content");
    //public virtual string? CharSet => null;

    //protected byte[]? Content = null;

    /*public virtual byte[] ReadAllBites() {
        if (Content == null) {
            Content = File.ReadAllBytes(ContentFilePath);
        }
        return Content;
    }

    public string ReadAllText() {
        var data = ReadAllBites();
        return FileUtils.ReadAllText(data, CharSet);
    }*/

    protected override void SetGenome(TGenome genome) {
        /*base.SetGenome(genome);
        KeyHash = Hash.CreateFromString(Key).ToString();
        CacheSubDirectory = Path.Combine(Cache.Directory, KeyHash);
        CachedDataJsonFilePath = Path.Combine(CacheSubDirectory, CachedDataJsonFileName);*/
    }


    


    


    


}


