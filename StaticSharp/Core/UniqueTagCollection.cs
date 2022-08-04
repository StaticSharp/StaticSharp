using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp.Gears {
    public class UniqueTagCollection {

        private string IdPrefix { get; }
        public UniqueTagCollection(string idPrefix) {
            IdPrefix = idPrefix;
        }

        private int currentIdIndex = 0;
        private Dictionary<string, KeyValuePair<int,TagGenerator>> items = new ();

        private string MakeId(int idIndex) {
            return IdPrefix + idIndex.ToString("X");
        }

        public string Add(TagGenerator tagGenerator) {

            lock (this) {
                if (items.TryGetValue(tagGenerator.Key, out var existing)) {
                    return MakeId(existing.Key);
                }
                var result = MakeId(currentIdIndex);
                items.Add(tagGenerator.Key, new(currentIdIndex, tagGenerator));
                currentIdIndex++;
                return result;
            }
        }

        public Task<IEnumerable<Tag>> GetAllAsync() {
            lock (this) {
                return items.Values.Select(x => x.Value.Generate(MakeId(x.Key))).SequentialOrParallel();
            }
        }

    }
}