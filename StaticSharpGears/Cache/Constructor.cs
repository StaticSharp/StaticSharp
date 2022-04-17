using System.Reflection;
using System.Text;

namespace StaticSharp.Gears;

public abstract record Constructor {

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

public abstract record Constructor<T> : Constructor, IKeyProvider where T : ICacheable{
    public T CreateOrGetCached() {
        T value = Cache.Get<T>(Key);
        if (value == null) {
            value = Create();
            value.AfterConstruction();
            Cache.Add(Key, value);
        }
        return value;
    }
    protected abstract T Create();    
}




