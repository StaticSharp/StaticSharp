using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp.Gears {

    /*public class Fonts {
        ConcurrentDictionary<string, CacheableFont> items = new();
        public void Register(string key, CacheableFont genome) {
            items.GetOrAdd(key, key => genome);
        }

        public async Task<IEnumerable<CacheableFont>> GetAllAsync() {
            var sorted = items.OrderBy(x => x.Key);
            return await Task.WhenAll(sorted.Select(async x => await x.Value));
        }

    }*/



    /*public class IncludesCache<TCacheable>: ConcurrentDictionary<string, TCacheable> where TCacheable : class {


        public TCacheable CreateOrGet(Genome<TCacheable> genome) {
            var key = genome.Key;
            TCacheable value;

            if (TryGetValue(key, out value)) {
                return value;
            } else {
                value = genome.Create();
                TryAdd(key, value);
                return value;
            }
        }
    }*/
}