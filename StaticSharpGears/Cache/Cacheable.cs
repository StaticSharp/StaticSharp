using System.Text.Json;
using System.Reflection;
namespace StaticSharp.Gears;

public abstract class Cacheable<TGenome> : ICacheable<TGenome>, IKeyProvider
    where TGenome : class, IKeyProvider {
    public TGenome Genome { get; private set; } = null!;
    public string Key { get; private set; } = null!;
    //public Task Job { get; protected set; } = null!;

    //public virtual IEnumerable<SecondaryTask>

    protected virtual void SetGenome(TGenome genome) {
        Genome = genome;
        Key = Genome.Key;
    }
    void ICacheable<TGenome>.SetGenome(TGenome genome) => SetGenome(genome);


    protected abstract Task CreateAsync();
    Task ICacheable<TGenome>.CreateAsync() => CreateAsync();

    

    /*protected virtual async Task CreateInternalAsync() {
        var exception = default(Exception);
        var secondaryTasks = GetSecondaryTasks();

        try {
            await CreateAsync();
        }
        catch (Exception e) {
            exception  = e;
        }
        
        foreach (var secondaryTask in secondaryTasks) {
            if (!secondaryTask.Value.IsCompleted) {
                if (exception == null) exception = new Exception($"SecondaryTask {secondaryTask.Key.Name} of object {GetType().FullName} is not completed.");
                secondaryTask.Value.SetException(exception);
            }
        }        
    }*/

    //protected abstract Task CreateAsync();

    /*public virtual void AfterConstruction() {
        Job = CreateInternalAsync();
    }*/

    /*private Dictionary<PropertyInfo, ISynchronouslyFailable> GetSecondaryTasks() {
        PropertyInfo[] properties = GetType().GetProperties(
            BindingFlags.Public
            | BindingFlags.NonPublic
            | BindingFlags.FlattenHierarchy
            | BindingFlags.Instance);

        Dictionary<PropertyInfo, ISynchronouslyFailable> secondaryTasks = new ();

        foreach (var p in properties) {
            if (typeof(ISynchronouslyFailable).IsAssignableFrom(p.PropertyType)) {
                var value = p.GetValue(this) as ISynchronouslyFailable;
                if (value == null)
                    throw new Exception($"SecondaryTask {p.Name} of object {GetType().FullName} is null");
                secondaryTasks.Add(p,value);
            }
        }
        return secondaryTasks;
    }*/


}

public abstract class Cacheable<Constructor, Data> : Cacheable<Constructor>
        where Constructor : class, IKeyProvider
        where Data : class, new()
    {

    private static readonly JsonSerializerOptions JsonSerializerOptions = new() {
        IncludeFields = true,
    };
    private static readonly string CachedDataJsonFileName = "data.json";

    protected string CachedDataJsonFilePath { get; private set; } = null!;

    protected string KeyHash { get; private set; } = null!;

    protected string CacheSubDirectory { get; private set; } = null!;

    protected string ContentFilePath => Path.Combine(CacheSubDirectory, "content");

    protected virtual Data CachedData { get; set; } = null!;

    /*protected override Task CreateJob() {
        return base.CreateJob();
    }*/

    protected override void SetGenome(Constructor arguments) {
        base.SetGenome(arguments);
        KeyHash = Hash.CreateFromString(Key).ToString();
        CacheSubDirectory = Path.Combine(Cache.Directory, KeyHash);
        CachedDataJsonFilePath = Path.Combine(CacheSubDirectory, CachedDataJsonFileName);
    }


    /*public override async Task CreateAsync() {
        if (File.Exists(CachedDataJsonFilePath)) {
            var json = File.ReadAllText(CachedDataJsonFilePath);
            CachedData = JsonSerializer.Deserialize<Data>(json, JsonSerializerOptions);
            Load();
            Job = Task.CompletedTask;
        } else {
            CachedData = new();
            return await CreateInternalAsync();
        }
    }


    protected async Task CreateInternalAsync() {
        Directory.CreateDirectory(CacheSubDirectory);
        await CreateAsync();
        StoreData();
    }

    protected abstract void Load();*/

    protected bool LoadData() {
        if (!File.Exists(CachedDataJsonFilePath)) return false;
        var json = File.ReadAllText(CachedDataJsonFilePath);
        CachedData = JsonSerializer.Deserialize<Data>(json, JsonSerializerOptions);
        return true;
    }

    protected void CreateCacheSubDirectory() {
        Directory.CreateDirectory(CacheSubDirectory);
    }


    protected void StoreData() {
        string json = JsonSerializer.Serialize(CachedData, JsonSerializerOptions);
        File.WriteAllText(CachedDataJsonFilePath, json);
    }


}


