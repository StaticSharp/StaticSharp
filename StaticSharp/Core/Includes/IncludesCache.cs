using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace StaticSharp.Gears {


    public class IncludesCache<TGenome, TCacheable>: ConcurrentDictionary<string, TCacheable>
        where TCacheable: Cacheable<TGenome>
        where TGenome : class, IKeyProvider, IGenome<TCacheable> {


        public async Task<TCacheable> CreateOrGet(TGenome genome) {
            var key = genome.Key;
            TCacheable value;

            if (TryGetValue(key, out value)) {
                return value;
            } else {
                value = await genome.CreateAsync();
                TryAdd(key, value);
                return value;
            }
        }
    }
}