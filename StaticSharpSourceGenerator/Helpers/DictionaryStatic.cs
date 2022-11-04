using System.Collections.Generic;

static class DictionaryStatic {
    public static TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> x, TKey key) where TValue: new() {
        TValue result;
        if (!x.TryGetValue(key, out result)) {
            result = new TValue();
            x.Add(key, result);
        }
        return result;
    }
}
