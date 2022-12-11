

using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaticSharp.Gears;


public class CacheItem { 
    public Genome Genome { get; set; }

    private object? value;

    public object? Value => value;
    public object ValueNotNull {
        get {
            if (value == null) {
                value = Genome.Create();
            }
            return value;
        }
    }

    private HashSet<string>? dependentKeys;

    public CacheItem(Genome genome) {
        Genome = genome;
    }

    public HashSet<string>? DependentKeys => dependentKeys;
    public HashSet<string> DependentKeysNotNull {
        get {
            if (dependentKeys == null) {
                dependentKeys = new HashSet<string>();
            }
            return dependentKeys;
        }
    }

}

public class Cache {

    public static string Directory { get; set; }


    static Dictionary<string, CacheItem> items = new();



    private static CacheItem CreateOrGetItem(Genome genome) {
        if (!items.TryGetValue(genome.Key, out var item)) {
            item = new CacheItem(genome);
            items.Add(genome.Key, item);
        }
        return item;
    }

    public static T CreateOrGet<T>(Genome<T> genome) where T: class {
        lock (items) {
            var item = CreateOrGetItem(genome);

            var sources = genome.Sources;
            if (sources != null) {
                foreach (var source in sources) {
                    var sourceItem = CreateOrGetItem(source);
                    sourceItem.DependentKeysNotNull.Add(genome.Key);
                }
            }

            return (T)item.ValueNotNull;
        }
    }


    private static void CollectMutatedItems(CacheItem root, HashSet<string> keysToDelete) {
        if (root.DependentKeys != null) {
            foreach (var key in root.DependentKeys) {
                keysToDelete.Add(key);
                CacheItem? branch = items.GetValueOrDefault(key);
                if (branch != null) {
                    CollectMutatedItems(branch, keysToDelete);
                }
            }
        }
    }

    public static void TrimMutatedItems() {
        lock (items) {
            var keysToDelete = new HashSet<string>();
            foreach (var i in items) {
                var cacheItem = i.Value;
                if (cacheItem.Value != null) {
                    if (cacheItem.Value is IMutableAsset mutableAsset) {
                        if (!mutableAsset.GetValid()) {
                            keysToDelete.Add(i.Key);
                            CollectMutatedItems(cacheItem, keysToDelete);
                        }
                    }
                }
            }
            foreach (var key in keysToDelete) {
                var item = items[key];
                item.Genome.DeleteCacheSubDirectory();
                items.Remove(key);
            }
        }    
    }


}






