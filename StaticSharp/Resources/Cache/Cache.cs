using StaticSharp.Gears;
using System.Text.Json;


namespace StaticSharp;

public class Cache {

    private class Item {
        public Func<bool>? Verify { get; init; }
        public object Value { get; init; }

        public Item(object value, Func<bool>? verify) {

            Verify = verify;
            Value = value;
        }
    }


    private static string? directory = null;
    public static string Directory {
        get {
            return directory ?? throw new InvalidOperationException("Cache.Directory is not set.");
        }
        set {
            directory = value;
        }
    }


    static Dictionary<string, Item> items = new();

    public delegate void Constructor<T>(out T value, out Func<bool>? verify) where T : class;

    public static T GetOrCreate<T>(string key, Constructor<T> constructor) where T : class {
        lock (items) {
            if (!items.TryGetValue(key, out var item)) {
                constructor(out var value, out var verify);
                item = new Item(value,verify);
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
        private string CacheSubDirectory => Path.Combine(Directory, KeyHash);
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
            if (!System.IO.Directory.Exists(CacheSubDirectory)) {
                System.IO.Directory.CreateDirectory(CacheSubDirectory);
            }
        }

        public void DeleteCacheSubDirectory() {
            if (!System.IO.Directory.Exists(CacheSubDirectory))
                return;
            System.IO.Directory.Delete(CacheSubDirectory, true);
            while (System.IO.Directory.Exists(CacheSubDirectory)) {
                Thread.Sleep(100);
            }
        }

    }


}






