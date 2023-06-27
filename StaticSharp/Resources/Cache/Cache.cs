

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace StaticSharp.Gears;



public interface ICacheItemConstructor {
    CacheItem Create();
}

public class CacheItem {

    //public string Key { get; init; }
    public Func<bool>? Verify { get; init; }
    public object Value { get; init; }

    public CacheItem(object value, Func<bool>? verify) {

        Verify = verify;
        Value = value;
    }

    /*public Genome Genome { get; set; }

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
    }*/

}

public class Cache {

    public static string RootDirectory { get; set; }


    static Dictionary<string, CacheItem> items = new();

    public delegate void Constructor<T>(out T value, out Func<bool>? verify) where T : class;

    public static T GetOrCreate<T>(string key, Constructor<T> constructor) where T : class {
        lock (items) {
            if (!items.TryGetValue(key, out var item)) {
                constructor(out var value, out var verify);
                item = new CacheItem(value,verify);
                items.Add(key, item);
                return value;
            }
            return (T)item.Value;
        }
    }



    public static void TrimMutatedItems() {
        lock (items) {

            int keysDeleted = 0;
            do {
                var keysToDelete = new HashSet<string>();
                var itemsCopy = items.ToArray();//Fix for dotnet hotReload
                foreach (var i in itemsCopy) {
                    if (i.Value.Verify != null) {
                        if (!i.Value.Verify()) {
                            keysToDelete.Add(i.Key);
                        }
                    }
                }
                keysDeleted = keysToDelete.Count;
                foreach (var key in keysToDelete) {
                    var item = items[key];
                    GetSlot(key).DeleteCacheSubDirectory();
                    items.Remove(key);
                }

            } while (keysDeleted > 0);
        }    
    }


    public static Slot GetSlot(string key) { 
        return new Slot(key);
    }

    public class Slot {

        string key;
        string? keyHash = null;
        string KeyHash {
            get {
                if (keyHash == null)
                    keyHash = Hash.CreateFromString(key).ToString();
                return keyHash;
            }        
        }

        public Slot(string key) {
            this.key = key;
        }

        private static readonly JsonSerializerOptions JsonSerializerOptions = new() {
            IncludeFields = true,
        };
        private static readonly string CachedDataJsonFileName = "data.json";
        private string CachedDataJsonFilePath => Path.Combine(CacheSubDirectory, CachedDataJsonFileName);
        private string CacheSubDirectory => Path.Combine(RootDirectory, KeyHash);
        private string ContentFilePath => Path.Combine(CacheSubDirectory, "content");

        public bool LoadData<T>(out T data) where T : new() {
            if (!File.Exists(CachedDataJsonFilePath)) {
                data = new();
                return false;
            }

            var json = FileUtils.ReadAllText(CachedDataJsonFilePath);
            var deserializationResult = JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
            if (deserializationResult == null) {
                data = new();
                return false;
            }
            data = deserializationResult;
            return true;
        }

        public bool ContentExists() {
            return File.Exists(ContentFilePath);
        }

        public byte[] LoadContent() {
            return FileUtils.ReadAllBytes(ContentFilePath);
        }

        public string LoadContentText() {
            return FileUtils.ReadAllText(ContentFilePath);
        }

        public Slot StoreData<T>(T data) {
            string json = JsonSerializer.Serialize(data, JsonSerializerOptions);
            StoreRawJson(json);
            return this;
        }

        private Slot StoreRawJson(string json) {
            CreateCacheSubDirectory();
            File.WriteAllText(CachedDataJsonFilePath, json);
            return this;
        }

        public Slot StoreContent(byte[] data) {
            CreateCacheSubDirectory();
            FileUtils.WriteAllBytes(ContentFilePath, data);
            return this;
        }

        public Slot StoreContentText(string text) {
            CreateCacheSubDirectory();
            FileUtils.WriteAllText (ContentFilePath, text);
            return this;
        }

        private void CreateCacheSubDirectory() {
            if (!Directory.Exists(CacheSubDirectory)) {
                Directory.CreateDirectory(CacheSubDirectory);
            }
        }

        public void DeleteCacheSubDirectory() {
            if (!Directory.Exists(CacheSubDirectory))
                return;
            Directory.Delete(CacheSubDirectory, true);
            while (Directory.Exists(CacheSubDirectory)) {
                Thread.Sleep(100);
            }
        }

    }


}






