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

        public virtual IEnumerable<Tag> GenerateHtml(Context context) {
            if (items != null) {
                foreach (var i in items) {
                    var child = i.GenerateHtml(context);
                    yield return child;
                    //parent.Add(child);
                }
            }
        }

        /*public virtual Tag GenerateHtml(Context context) {
            var result = new Tag();
            GenerateHtml(result, context);
            return result;
        }*/



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