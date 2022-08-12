using StaticSharp.Gears;
using System.Collections.Generic;
using System.Linq;

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
    }

}