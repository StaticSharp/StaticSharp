using AngleSharp.Dom;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace StaticSharp.Gears;

public abstract record Genome: KeyProvider {

    //public abstract object Create();





    

    
}




public abstract record Genome<TCacheable> : Genome where TCacheable : class {


    protected abstract void Create(out TCacheable value, out Func<bool>? verify);
    CacheItem Create() {
        Create(out var value, out var verify);
        return new CacheItem(value, verify);
    }

    public TCacheable Get() {
        return (TCacheable)Cache.GetOrCreate(Key,Create).Value;    
    }


}







