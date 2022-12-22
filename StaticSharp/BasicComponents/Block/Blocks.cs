using AngleSharp.Dom;
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp {




    public class Blocks : IBlockCollector, IEnumerable<IBlock> {

        private List<KeyValuePair<string?, IBlock>>? items;
        public Blocks() { }
        public Blocks(Blocks other){
            items = other.items;
        }

        //public IEnumerable<KeyValuePair<string?, IBlock>> Items => items ?? Enumerable.Empty<KeyValuePair<string?, IBlock>>();


        public void Add(string? propertyName, IBlock? value) {
            if (value != null) {
                if (items == null)
                    items = new();
                items.Add(new KeyValuePair<string?, IBlock>(propertyName, value));
            }            
        }

        public virtual void GenerateHtml(Tag parent, Context context) {
            if (items != null) {
                foreach (var i in items) {
                    var child = i.Value.GenerateHtml(context, new Role(true, i.Key));
                    parent.Add(child);
                }
            }
        }

        /*public virtual Tag GenerateHtml(Context context) {
            var result = new Tag();
            GenerateHtml(result, context);
            return result;
        }*/



        IEnumerator<IBlock> IEnumerable<IBlock>.GetEnumerator() {
            if (items != null)
                return items.Select(x => x.Value).GetEnumerator();
            return Enumerable.Empty<IBlock>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (this as IEnumerable<IBlock>).GetEnumerator();
        }
    }

}