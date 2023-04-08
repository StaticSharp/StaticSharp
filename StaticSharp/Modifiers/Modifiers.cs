
using System.Collections;

namespace StaticSharp.Gears {
    public class Modifiers : IEnumerable<Modifier> {

        private List<Modifier>? items;
        public Modifiers() { }
        public Modifiers(Modifiers other) {
            items = other.items;
        }
        public void Add(Modifier? value) {
            if (value != null) {
                if (items == null)
                    items = new();
                items.Add(value);
            }
        }

        IEnumerator<Modifier> IEnumerable<Modifier>.GetEnumerator() {
            if (items != null)
                return items.GetEnumerator();
            return Enumerable.Empty<Modifier>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (this as IEnumerable<Modifier>).GetEnumerator();
        }
    }

}


