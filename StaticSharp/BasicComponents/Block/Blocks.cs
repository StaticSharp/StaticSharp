using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections;


namespace StaticSharp {




    public class Blocks : IBlockCollector, IEnumerable<Block> {

        private List<Block>? items;
        public Blocks() { }
        public Blocks(Blocks other){
            items = other.items;
        }
        public void Add(Block? value) {
            if (value != null) {
                if (items == null)
                    items = new();
                items.Add(value);
            }            
        }

        IEnumerator<Block> IEnumerable<Block>.GetEnumerator() {
            if (items != null)
                return items.GetEnumerator();
            return Enumerable.Empty<Block>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (this as IEnumerable<Block>).GetEnumerator();
        }
    }

}