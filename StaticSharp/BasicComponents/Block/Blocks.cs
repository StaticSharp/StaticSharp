using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp {




    public class Blocks : List<KeyValuePair<string?, IBlock>>, IBlockCollector {
        public Blocks(): base() { }
        public Blocks(Blocks other): base(other) {}

        public IEnumerable<IBlock> Values => this.Select(x => x.Value);
        public void Add(string? propertyName, IBlock? value) {
            if (value != null) {
                Add(new KeyValuePair<string?, IBlock>(propertyName, value));
            }            
        }

        public virtual async Task<Tag> GenerateHtmlAsync(Context context) {
            var result = new Tag();
            foreach (var i in this) {
                var child = await i.Value.GenerateHtmlAsync(context, new Role(false, i.Key));
                result.Add(child);
            }
            return result;
        }


    }

}