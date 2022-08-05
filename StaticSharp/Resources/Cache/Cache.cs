using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StaticSharp.Gears;

public static partial class Cache { 

    public static string Directory { get; set; }

    /*public static Task<TResource> AddOrGetAsync<TResource>(string key, Func<TResource> resource) {
        throw new NotImplementedException();
    }*/

    private static readonly Dictionary<string, Task<object>> items = new();



    public static void Lock() {
        Monitor.Enter(items);
    }

    public static void Unlock() {
        Monitor.Exit(items);
    }

    public static Task<object>? Get(string key) {
        if (items.TryGetValue(key, out var resultTask)) {
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




