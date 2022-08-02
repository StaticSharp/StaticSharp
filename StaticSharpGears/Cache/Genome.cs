using System.Reflection;
using System.Text;

namespace StaticSharp.Gears;

public interface IGenome<TCacheable>: IKeyProvider {
    Task<TCacheable> CreateOrGetCached();
    Task<TCacheable> CreateAsync();
}

public abstract record Genome: IKeyProvider {

    public string Key { get; }

    protected Genome() {
        Key = CalculateKey();
    }
    private string CalculateKey() {
        var type = GetType();



        

        var stringBuilder = new StringBuilder();
        stringBuilder.Append(GetType().FullName);


        while (type != typeof(Genome)) {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields) {
                var fieldKey = GetFieldKey(field);
                stringBuilder.Append('\0').Append(fieldKey);
            }

            type = type.BaseType;
        }


        
        

        var key = stringBuilder.ToString();
        return key;
    }

    private string GetFieldKey(FieldInfo fileInfo) {
        var value = fileInfo.GetValue(this);
        return KeyUtils.GetKeyForObject(value);
    }

    public override string ToString() {
        return Key;
    }
}

public abstract record Genome<TFinalGenome,TCacheable> : Genome, IGenome<TCacheable>
    where TFinalGenome : Genome<TFinalGenome, TCacheable>
    where TCacheable : ICacheable<TFinalGenome>, new() {

    public async Task<TCacheable> CreateOrGetCached() {


        //Cache.Lock();

        try {
            Task<object>? task = Cache.Get(Key);
            if (task == null) {
                task = CreateAsync().ContinueWith(x => (object)x.Result);
                Cache.Add(Key, task);
                return (TCacheable)(await task);
            }
            
            var value = (TCacheable)(await task);
            if (value is IMutableAsset mutable) {
                if (!await mutable.GetValidAsync()) {
                    mutable.DeleteCacheSubDirectory();
                    task = CreateAsync().ContinueWith(x => (object)x.Result);
                    Cache.Add(Key, task);
                }
            } 
            
            return (TCacheable)(await task);
        }
        finally {

            //Cache.Unlock();
        }

        
    }

    public async Task<TCacheable> CreateAsync() {
        var result = new TCacheable();
        result.SetGenome((TFinalGenome)this);
        await result.CreateAsync();
        return result;
    }    
}

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







