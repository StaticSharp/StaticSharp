namespace StaticSharpGears;

public static class Assets {
    public static string Directory { get; set; }

    private static readonly Dictionary<string, IAsset> itemsStoringTasks = new();
    public static void Add(string key, IAsset item) {
        if (!itemsStoringTasks.ContainsKey(key)) {
            //items[key] = item;
            itemsStoringTasks[key] = item;
        }
    }

    public static async Task StoreAsync() {
        await Task.WhenAll(itemsStoringTasks.Values.Select(x=>x.StoreAsync(Directory)).ToArray());
    }


}




