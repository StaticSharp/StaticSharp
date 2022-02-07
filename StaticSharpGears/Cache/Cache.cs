namespace StaticSharpGears;

public static partial class Cache { 

    public static string Directory { get; set; }

    /*public static Task<TResource> AddOrGetAsync<TResource>(string key, Func<TResource> resource) {
        throw new NotImplementedException();
    }*/

    private static readonly Dictionary<string,ICacheable> items = new();




    public static T? Get<T>(string key) {
        if (items.TryGetValue(key, out var result)) { 
            return (T)result;
        }
        return default(T);
    }

    public static void Add(string key, ICacheable value) { 
        items[key] = value;
    }

    

    public static async Task WaitAsync() {
        await Task.WhenAll(items.Values.Select(x => x.Job));
    }

    /*static Cache() {
        AppDomain.CurrentDomain.ProcessExit +=
            Destructor;
    }

    static void Destructor(object? sender, EventArgs e) {
        // clean it up
    }*/

}




