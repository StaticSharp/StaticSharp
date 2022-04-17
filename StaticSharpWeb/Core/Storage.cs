using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
/*
namespace StaticSharp.Gears {

    public interface IStorage {
        public string StorageDirectory { get; }
        public string IntermediateCache { get; }

        Task<TResource> AddOrGetAsync<TResource>(string key, Func<TResource> resource) where TResource : class, IResource;
    }

    public class Storage : IStorage {

        public Storage(string storageDirectory, string intermediateCache) 
            => (StorageDirectory, IntermediateCache) = (storageDirectory, intermediateCache);
        

        public string StorageDirectory { get; }

        public string IntermediateCache { get; }

        private readonly ConcurrentDictionary<object, SemaphoreSlim> _locks = new();
        private readonly List<IResource> _cache = new();

        public async Task<TResource> AddOrGetAsync<TResource>(string key, Func<TResource> resource) where TResource : class, IResource {
            var mylock = _locks.GetOrAdd(key, _ => new(1, 1));
            await mylock.WaitAsync();
            IResource result;
            try {
                result = _cache.Find(x => x.Key == key);
                if (result == null) {
                    result = resource();
                    await result.GenerateAsync();
                    _cache.Add(result);
                }
            } finally {
                mylock.Release();
            }
            return result as TResource;
        }
    }
}*/