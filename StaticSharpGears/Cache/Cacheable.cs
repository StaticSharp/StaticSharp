using System.Text.Json;
using System.Reflection;
namespace StaticSharp.Gears;

public abstract class Cacheable<Constructor> : ICacheable, IKeyProvider
    where Constructor : IKeyProvider {
    protected Constructor Arguments { get; }
    public string Key { get; }
    public Task Job { get; protected set; } = null!;

    //public virtual IEnumerable<SecondaryTask>

    protected Cacheable(Constructor arguments) {
        Arguments = arguments;
        Key = Arguments.Key;
        //Job = CreateJob();
    }




    protected virtual async Task CreateInternalAsync() {
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
    }

    protected abstract Task CreateAsync();

    public virtual void AfterConstruction() {
        Job = CreateInternalAsync();
    }

    private Dictionary<PropertyInfo, ISynchronouslyFailable> GetSecondaryTasks() {
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
    } 

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

    protected virtual Data? CachedData { get; set; }

    /*protected override Task CreateJob() {
        return base.CreateJob();
    }*/

    protected Cacheable(Constructor arguments) : base(arguments) {        
        KeyHash = Hash.CreateFromString(Key).ToString();
        CacheSubDirectory = Path.Combine(Cache.Directory, KeyHash);
        CachedDataJsonFilePath = Path.Combine(CacheSubDirectory, CachedDataJsonFileName);
    }

    /*public override void AfterConstruction() {
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


    protected async Task CreateInternalAsync() {
        Directory.CreateDirectory(CacheSubDirectory);
        await CreateAsync();
        StoreData();
    }*/

    //protected abstract void Load();

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


