using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace StaticSharp.Gears;

/*public interface IGenome<TCacheable>: IKeyProvider {
    Task<TCacheable> CreateOrGetCached();
    Task<TCacheable> CreateAsync();
}*/

public abstract record Genome: IKeyProvider {
    public string Key { get; }
    protected Genome() {
        Key = CalculateKey();
    }
    private string CalculateKey() {
        var type = GetType();

        var stringBuilder = new StringBuilder();
        stringBuilder.Append(GetType().FullName);


        while (type != typeof(Genome)) {
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




public abstract record Genome<TCacheable> : Genome {

    private static readonly JsonSerializerOptions JsonSerializerOptions = new() {
        IncludeFields = true,
       
    };
    private static readonly string CachedDataJsonFileName = "data.json";
    protected string CachedDataJsonFilePath => Path.Combine(CacheSubDirectory, CachedDataJsonFileName);
    protected string CacheSubDirectory => Path.Combine(Cache.Directory, KeyHash);
    protected string ContentFilePath => Path.Combine(CacheSubDirectory, "content");
    protected string KeyHash => Hash.CreateFromString(Key).ToString();


    public async Task<TCacheable> CreateOrGetCached() {

        using (await Cache.AsyncLock.LockAsync()) {
            try {
                Task<object>? task = Cache.Get(Key);
                if (task == null) {
                    task = CreateAsync().ContinueWith(x => (object)x.Result);
                    Cache.Add(Key, task);
                    return (TCacheable)(await task);
                }

                var value = (TCacheable)(await task);
                if (value is IMutableAsset mutable) {
                    if (!await mutable.GetValidAsync()) {
                        mutable.DeleteCacheSubDirectory();
                        task = CreateAsync().ContinueWith(x => (object)x.Result);
                        Cache.Add(Key, task);
                    }
                }

                return (TCacheable)(await task);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw e;
                //Cache.Unlock();
            }
        }
    }

    protected bool LoadData<T>(out T data) where T : new() {
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

    protected void StoreData<T>(T data) {
        string json = JsonSerializer.Serialize(data, JsonSerializerOptions);
        File.WriteAllText(CachedDataJsonFilePath, json);
    }

    protected void CreateCacheSubDirectory() {
        Directory.CreateDirectory(CacheSubDirectory);
    }

    public void DeleteCacheSubDirectory() {
        DeleteDirectory(CacheSubDirectory);
    }

    private void DeleteDirectory(string directoryPath) {
        DirectoryInfo dir = new DirectoryInfo(directoryPath);

        foreach (FileInfo file in dir.GetFiles()) {
            file.Delete();
        }

        foreach (DirectoryInfo directory in dir.GetDirectories()) {
            DeleteDirectory(directory.FullName);
            directory.Delete();
            while (directory.Exists) {
                Thread.Sleep(100);
            }
        }
    }


    public abstract Task<TCacheable> CreateAsync(); /*{
        var result = new TCacheable();
        result.SetGenome((TFinalGenome)this);
        await result.CreateAsync();
        return result;
    }    */
}







