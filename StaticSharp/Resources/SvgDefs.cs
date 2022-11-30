using StaticSharp.Html;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharp.Gears {
    public class SvgDefs {
        public ConcurrentDictionary<string, Tag> Items { get; } = new();

        public string Add(Tag tag) {
            var key = tag.Id;
            if (string.IsNullOrEmpty(key))
                throw new System.Exception("Key required");
            Items.GetOrAdd(key, tag);
            return key;
        }

        public IEnumerable<Tag> GetOrderedItems() {
            return Items.Values.OrderBy(x => x.Id).ToArray();
        }
    }
}