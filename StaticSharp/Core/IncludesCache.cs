using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace StaticSharp.Gears {


    public class IncludesCache<TCacheable>: ConcurrentDictionary<string, TCacheable>{


        public async Task<TCacheable> CreateOrGet(Genome<TCacheable> genome) {
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