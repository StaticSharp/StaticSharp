using System.Reflection;
using System.Text;

namespace StaticSharp.Gears;

public static partial class KeyUtils {

    public static string Combine<T>(params object?[] state) {
        return Combine(typeof(T), state);
    }
    public static string Combine(Type container, params object?[] state) {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(container.Name);
        foreach (var i in state) {
            stringBuilder.Append('\0').Append(GetKeyForObject(i));
        }
        return stringBuilder.ToString();
    }


    public static string GetKeyForObject(object? value) {
        if (value == null) {
            return "";
        }
        if (value is string valueAsString) { 
            return valueAsString;
        }
        if (value is bool valueAsBool) {
            return valueAsBool.ToString().ToLower();
        }

        if (value.GetType().IsEnum) {
            return value.ToString();
        }

        if (value is IKeyProvider keyProvider) {
            return keyProvider.Key;
        }

        

        var getKeyMethod = typeof(KeyCalculators).GetMethod("GetKey", BindingFlags.Static | BindingFlags.Public, new Type[] { value.GetType() });
        if (getKeyMethod != null) {
            var keyObject = getKeyMethod.Invoke(null, new object[] { value });
            if (keyObject is string key) {
                return key;
            }
        }

        

        throw new NotImplementedException($"GetKey() not found. Object : {value.GetType().FullName}");
    }


}




