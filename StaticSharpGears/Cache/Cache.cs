namespace StaticSharp.Gears;

public static partial class Cache { 

    public static string Directory { get; set; }

    /*public static Task<TResource> AddOrGetAsync<TResource>(string key, Func<TResource> resource) {
        throw new NotImplementedException();
    }*/

    private static readonly Dictionary<string, Task<object>> items = new();



/*    private static async Task<T> TaskCast<F,T>(Task<F> value){
        object v = await value;
        return (T)v;
    }*/
    public static Task<object>? Get(string key) {
        if (items.TryGetValue(key, out var result)) { 
            return result;
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




