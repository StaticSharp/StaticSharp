using System.Reflection;
using System.Text;

namespace StaticSharp.Gears;

public interface IGenome<T> {
    public Task<T> CreateOrGetCached();
}

public abstract record Genome: IKeyProvider {

    public string Key { get; }

    protected Genome() {
        Key = CalculateKey();
    }
    private string CalculateKey() {
        var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        var stringBuilder = new StringBuilder();
        stringBuilder.Append(GetType().FullName);
        foreach (var field in fields) {
            stringBuilder.Append('\0').Append(GetFieldKey(field));
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

    public Task<TCacheable> CreateOrGetCached() {
        //TODO add cache lock
        Task<object>? value = Cache.Get(Key);
        if (value == null) {
            value = Create().ContinueWith(x=>(object)x.Result);
            //value.AfterConstruction();
            Cache.Add(Key, value);
        }
        return value.ContinueWith(x=>(TCacheable)x.Result);
    }

    protected async Task<TCacheable> Create() {
        var result = new TCacheable();
        result.SetGenome((TFinalGenome)this);
        await result.CreateAsync();
        return result;
    }    
}

public abstract record AssetGenome<TFinalGenome, TCacheable> : Genome<TFinalGenome, TCacheable>, IGenome<IAsset>
    where TFinalGenome : AssetGenome<TFinalGenome, TCacheable>
    where TCacheable : ICacheable<TFinalGenome>, IAsset, new() {

    async Task<IAsset> IGenome<IAsset>.CreateOrGetCached() {
        return await base.CreateOrGetCached();
    }
}


