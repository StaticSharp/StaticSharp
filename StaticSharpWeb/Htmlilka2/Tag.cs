using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsmlWeb.Html {


    public class Collection<T> : ICollection<T> {
        protected readonly List<T> _innerList = new();
        public int Count => _innerList.Count;

        public bool IsReadOnly => false;

        public void Add(T item) {
            if(item == null)
                return;
            _innerList.Add(item);

        }

        public void Clear() => _innerList.Clear();

        public bool Contains(T item) => _innerList.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _innerList.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _innerList.GetEnumerator();

        public bool Remove(T item) => _innerList.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => _innerList.GetEnumerator();
    }


    public class Tag : Collection<INode>, INode {


        private static readonly HashSet<string> VoidElements = new() {
            "area",
            "base",
            "br",
            "col",
            "command",
            "embed",
            "hr",
            "img",
            "input",
            "keygen",
            "link",
            "meta",
            "param",
            "source",
            "track",
            "wbr",
            "!doctype",
        };

        public string Name { get; }


        public Attributes Attributes { get; private set; }


        public Tag(string name, object attributes = null) {
            Attributes = new Attributes(attributes);
            Name = name;
        }

        public void Add(string item) {
            if(string.IsNullOrEmpty(item)) { return;}

            Add(new TextNode(item));
        }


        public void WriteHtml(StringBuilder builder) {
            if(Name != null) {
                builder.Append('<').Append(Name);
                if(Attributes != null) { builder.Append(Attributes); }
                builder.Append('>');

                if(!VoidElements.Contains(Name.ToLower())) {
                    foreach(var node in this) {
                        node.WriteHtml(builder);
                    }
                    builder.Append("</").Append(Name).Append('>');
                }
            } else {
                foreach(var node in this) {
                    node.WriteHtml(builder);
                }
            }
        }


    }
}