using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp {




    public class Blocks : List<KeyValuePair<HtmlModifier?, IBlock>>, IBlockCollector {
        public Blocks(): base() { }
        public Blocks(Blocks other): base(other) {}

        public IEnumerable<IBlock> Values => this.Select(x => x.Value);
        public void Add(HtmlModifier? id, IBlock? block) {
            if (block != null) {
                //base.Add(id, block);
                Add(new KeyValuePair<HtmlModifier?, IBlock>(id, block));
            }            
        }

        public virtual async Task<Tag> GenerateHtmlAsync(Context context) {
            var result = new Tag();
            foreach (var i in this) {
                var child = await i.Value.GenerateHtmlAsync(context);
                if (i.Key != null) {
                    await i.Key.Apply(child);
                }
                result.Add(child);
            }
            return result;
        }


    }

}