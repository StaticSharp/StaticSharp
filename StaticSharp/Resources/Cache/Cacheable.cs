using System.Text.Json;

using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

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

public abstract class Cacheable<TGenome, TData> : Cacheable<TGenome>
        where TGenome : class, IKeyProvider
        where TData : class, new()
    {

    private static readonly JsonSerializerOptions JsonSerializerOptions = new() {
        IncludeFields = true,
    };
    private static readonly string CachedDataJsonFileName = "data.json";

    protected string CachedDataJsonFilePath { get; private set; } = null!;

    protected string KeyHash { get; private set; } = null!;

    protected string CacheSubDirectory { get; private set; } = null!;

    protected virtual string ContentFilePath => Path.Combine(CacheSubDirectory, "content");

    protected virtual TData CachedData { get; set; } = null!;

    public virtual string? CharSet => null;

    protected byte[]? Content = null;

    public virtual byte[] ReadAllBites() {
        if (Content == null) {
            Content = File.ReadAllBytes(ContentFilePath);
        }
        return Content;
    }

    public string ReadAllText() {
        

        var data = ReadAllBites();
        return FileUtils.ReadAllText(data, CharSet);


        /*Encoding encoding = (CharSet == null) ? Encoding.UTF8 : Encoding.GetEncoding(CharSet);
        
        var text = encoding.GetString(data);
        char first = text[0];
        return text;*/
    }


    /*protected override Task CreateJob() {
        return base.CreateJob();
    }*/

    protected override void SetGenome(TGenome genome) {
        base.SetGenome(genome);
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

    protected bool LoadData(/*bool contentFileMustExist*/) {
        if (!File.Exists(CachedDataJsonFilePath)) return false;

        /*if (contentFileMustExist) {
            if (!File.Exists(ContentFilePath)) return false;
        }*/

        var json = FileUtils.ReadAllText(CachedDataJsonFilePath);
        CachedData = JsonSerializer.Deserialize<TData>(json, JsonSerializerOptions);

        

        return true;
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


    protected void StoreData() {
        string json = JsonSerializer.Serialize(CachedData, JsonSerializerOptions);
        File.WriteAllText(CachedDataJsonFilePath, json);
    }


}


