namespace StaticSharp.Gears;

public static partial class Cache { 

    public static string Directory { get; set; }

    /*public static Task<TResource> AddOrGetAsync<TResource>(string key, Func<TResource> resource) {
        throw new NotImplementedException();
    }*/

    private static readonly Dictionary<string, Task<object>> items = new();

    //private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    private static Mutex mutex;

    /*    private static async Task<T> TaskCast<F,T>(Task<F> value){
            object v = await value;
            return (T)v;
        }*/

    public static void Lock() {
        Monitor.Enter(items);
    }

    public static void Unlock() {
        Monitor.Exit(items);
    }

    public static Task<object>? Get(string key) {
        if (items.TryGetValue(key, out var resultTask)) {
            
            /*var result = await resultTask;

            if (result is IMutableAsset mutableAsset) {
                if (!mutableAsset.Valid) {
                    items.Remove(key);
                    return null;
                }
            }*/

            return resultTask;
        }
        return null;


        
    }




    public static void Add(string key, Task<object> value) {
        items[key] = value;        
    }

    

/*    public static async Task WaitAsync() {
        await Task.WhenAll(items.Values.Select(x => x.Job));
    }*/

    /*static Cache() {
        AppDomain.CurrentDomain.ProcessExit +=
            Destructor;
    }

    static void Destructor(object? sender, EventArgs e) {
        // clean it up
    }*/

}




