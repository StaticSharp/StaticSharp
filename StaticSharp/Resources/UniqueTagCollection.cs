using StaticSharp;

using StaticSharp.Html;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp.Gears {




    public class SvgDefs {
        ConcurrentDictionary<string, Task<Tag>> items = new();
        public string Add(Genome<Task<Tag>> genome) {
            items.GetOrAdd(genome.Key, key => genome.CreateOrGetCached());
            return genome.Key;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync() {
            return await Task.WhenAll(items.Values.Select(async x => await x));
        }

    }


    /*public class UniqueTagCollection {

        private string IdPrefix { get; }
        public UniqueTagCollection(string idPrefix) {
            IdPrefix = idPrefix;
        }

        private int currentIdIndex = 0;
        private Dictionary<string, KeyValuePair<int, TagGenerator>> items = new();

        private string MakeId(int idIndex) {
            return IdPrefix + idIndex.ToString("X");
        }


        public string Add(TagGenerator tagGenerator) {
            if (items.TryGetValue(tagGenerator.Key, out var existing)) {
                return MakeId(existing.Key);
            }
            var result = MakeId(currentIdIndex);
            items.Add(tagGenerator.Key, new(currentIdIndex, tagGenerator));
            currentIdIndex++;
            return result;            
        }

        public async Task<IEnumerable<Tag>> GetAllAsync() {
            return await Task.WhenAll(items.Values.Select(async x => await x.Value.Generate(MakeId(x.Key))));
        }

    }*/
}