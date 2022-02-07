using System.Text.Json;

namespace StaticSharpGears;

public abstract class Cacheable<Constructor> : ICacheable, IKeyProvider
    where Constructor : IKeyProvider {
    protected Constructor Arguments { get; }
    public string Key { get; }
    public Task Job { get; protected set; }


    protected Cacheable(Constructor arguments) {
        Arguments = arguments;
        Key = Arguments.Key;
    }

    protected virtual async Task CreateInternalAsync() {
        await CreateAsync();
    }

    protected abstract Task CreateAsync();

}

public abstract class Cacheable<Constructor, Data> : Cacheable<Constructor>
        where Constructor : IKeyProvider
        where Data : class, new()
    {

    private static readonly JsonSerializerOptions JsonSerializerOptions = new() {
        IncludeFields = true,
    };
    private static readonly string CachedDataJsonFileName = "data.json";

    protected string CachedDataJsonFilePath { get; }

    protected string KeyHash { get; }

    protected string CacheSubDirectory { get; }


    protected Cacheable(Constructor arguments) : base(arguments) {        
        
        KeyHash = Hash.CreateFromString(Key).ToString();
        CacheSubDirectory = Path.Combine(Cache.Directory, KeyHash);

        CachedDataJsonFilePath = Path.Combine(CacheSubDirectory, CachedDataJsonFileName);

        if (File.Exists(CachedDataJsonFilePath)) {
            var json = File.ReadAllText(CachedDataJsonFilePath);
            CachedData = JsonSerializer.Deserialize<Data>(json, JsonSerializerOptions);
            Load();
            Job = Task.CompletedTask;
        } else {
            CachedData = new();
            Job = CreateInternalAsync();
        }
    }
    protected virtual Data CachedData { get; private set; }


    protected override async Task CreateInternalAsync() {
        Directory.CreateDirectory(CacheSubDirectory);
        await CreateAsync();
        StoreData();
    }

    protected abstract void Load();    

    private void StoreData() {
        string json = JsonSerializer.Serialize(CachedData, JsonSerializerOptions);
        File.WriteAllText(CachedDataJsonFilePath, json);
    }


}


