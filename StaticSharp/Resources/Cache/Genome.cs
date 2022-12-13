using System;

namespace StaticSharp.Gears;

public abstract record Genome<TCacheable>: KeyProvider where TCacheable : class {
    protected abstract void Create(out TCacheable value, out Func<bool>? verify);
    public TCacheable Result => Cache.GetOrCreate<TCacheable>(Key, Create);
}







