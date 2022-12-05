using System.Reflection;
using System.Text;

namespace StaticSharp.Gears;

public abstract record KeyProvider: IKeyProvider {

    private string? key = null;
    public string Key {
        get {
            if (key == null) {
                key = CalculateKey();
            }
            return key;
        }
    }

    private string CalculateKey() {
        var type = GetType();

        var stringBuilder = new StringBuilder();
        stringBuilder.Append(GetType().FullName);


        while (type != typeof(KeyProvider)) {
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







