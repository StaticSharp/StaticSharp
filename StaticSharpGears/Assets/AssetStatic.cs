namespace StaticSharp.Gears;

public static class AssetStatic {
    public static void RequireStored<T>(this T _this) where T: IAsset, IKeyProvider {
        Assets.Add(_this.Key, _this);
    }
}




