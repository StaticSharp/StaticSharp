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
        public void Add(string? id, IBlock? block) {
            if (block != null) {
                //base.Add(id, block);
                Add(new KeyValuePair<string?, IBlock>(id, block));
            }            
        }

        public virtual async Task<Tag> GenerateHtmlAsync(Context context) {
            var result = new Tag();
            foreach (var i in this) {
                result.Add(await i.Value.GenerateHtmlAsync(context, i.Key));
            }
            return result;
        }


    }

}