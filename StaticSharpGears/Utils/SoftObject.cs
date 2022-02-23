namespace StaticSharpGears;

public static class SoftObject {
    public static IDictionary<string,object>? MergeObjects(object? a, object? b) {
        

        var dA = ObjectToDictionary(a);
        var dB = ObjectToDictionary(b);
        if (dA == null) return dB;
        if (dB == null) return dA;

        var result = new Dictionary<string,object>(dA);
        foreach (var i in dB) {
            result.TryAdd(i.Key, i.Value);
        }
        return result;
    }


    public static IDictionary<string, object>? ObjectToDictionary(object? @object) {
        if (@object == null || Equals(@object, new { }))
            return null;

        if (@object is IDictionary<string, object> dictionary) {
            return dictionary;
        }

        Dictionary<string, object> result = new();
        foreach (var i in @object.GetType().GetProperties()) {
            var value = i.GetValue(@object);
            if (value != null) {
                result.Add(i.Name, value);
            }
        }
        return result;
    }

}




