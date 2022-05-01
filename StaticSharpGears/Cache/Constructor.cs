using System.Reflection;
using System.Text;

namespace StaticSharp.Gears;

public abstract record Constructor: IKeyProvider {

    public string Key { get; }

    protected Constructor() {
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

public abstract record Constructor<TFinalConstructor,Cacheable> : Constructor
    where TFinalConstructor : Constructor<TFinalConstructor, Cacheable>
    where Cacheable : ICacheable<TFinalConstructor>, new() {
    public Task<Cacheable> CreateOrGetCached() {
        //TODO add cache lock
        Task<object>? value = Cache.Get(Key);
        if (value == null) {
            value = Create().ContinueWith(x=>(object)x.Result);
            //value.AfterConstruction();
            Cache.Add(Key, value);
        }
        return value.ContinueWith(x=>(Cacheable)x.Result);
    }

    protected async Task<Cacheable> Create() {
        var result = new Cacheable();
        result.SetArguments((TFinalConstructor)this);
        await result.CreateAsync();
        return result;
    }    
}




