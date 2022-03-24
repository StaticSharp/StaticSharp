using StaticSharpGears;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace StaticSharpWeb.Html {


    /*public class Collection<T> : IEnumerable, ICollection<T> {
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
    }*/


    public class Tag : IEnumerable, INode {
        protected readonly List<INode> children = new();

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

        public string? Name { get; set; }//null tag is a collection


        public IDictionary<string, object>? Attributes { get; private set; }

        public IDictionary<string, object> AttributesNotNull {
            get {
                if (Attributes == null) {
                    Attributes = new Dictionary<string,object>();
                }
                return Attributes;
            }
        }

        public Tag(string? name, IDictionary<string, object> attributes) {
            Attributes = attributes;
            Name = name;
        }

        public Tag(string? name, object? attributes = null) {
            if (attributes!=null)
                Attributes = SoftObject.ObjectToDictionary(attributes);
            Name = name;
        }

        public void Add(string item) {
            if(string.IsNullOrEmpty(item)) { return;}

            Add(new TextNode(item));
        }

        public void Add(INode? item) {
            if (item == null)
                return;
            children.Add(item);
        }

        public void Add(IEnumerable<INode?> items) {
            foreach (INode? item in items) {
                Add(item);
            }            
        }

        public virtual void WriteHtml(StringBuilder builder) {
            //System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            /*string? objectToString(object? obj) {
                return obj switch {
                    //string => obj.,
                    float => ((float)obj).ToString(CultureInfo.InvariantCulture),
                    float => ((float)obj).ToString(CultureInfo.InvariantCulture),
                    _ => "Watching TV",
                };
                //obj.GetType
            }*/

            void WriteCssStyle(StringBuilder builder, object styleObject) {
                if (styleObject is string styleObjectString) {
                    builder.Append(styleObjectString);
                } else {
                    var styleDictionary = SoftObject.ObjectToDictionary(styleObject);
                    if (styleDictionary == null) return;

                    foreach (var item in styleDictionary) {
                        string? stringValue;
                        if (item.Value is Color color) {
                            stringValue = ColorTranslator.ToHtml(color);
                        } else {
                            stringValue = item.Value.ToString();
                        }
                        if (stringValue == null) continue;

                        builder.Append($"{StaticSharpGears.CaseConverter.PascalToKebabCase(item.Key)}:{stringValue};");
                    }
                }
            }


            void WriteAttributes(StringBuilder builder) {
                foreach (var i in Attributes) {
                    if (i.Key.ToLower() == "style") {
                        if (i.Value != null) {
                            builder.Append(" style = \"");
                            WriteCssStyle(builder, i.Value);
                            builder.Append("\"");
                        }
                    } else {
                        var valueString = i.Value?.ToString();
                        

                        if (valueString != null) {
                            var kebebKey = StaticSharpGears.CaseConverter.PascalToKebabCase(i.Key);
                            if (valueString.Length > 0) {                                
                                builder.Append($" {kebebKey}=\"{valueString.ReplaceInvalidAttributeValueSymbols()}\"");
                            } else {
                                builder.Append(' ').Append(kebebKey);
                            }
                        }
                    }
                }
            }



            if (Name != null) {
                builder.Append('<').Append(Name);
                if (Attributes != null)
                    WriteAttributes(builder);
                builder.Append('>');

                if(!VoidElements.Contains(Name.ToLower())) {
                    foreach(var node in children) {
                        node.WriteHtml(builder);
                    }
                    builder.Append("</").Append(Name).Append('>');
                }
            } else {
                foreach(var node in children) {
                    node.WriteHtml(builder);
                }
            }
        }




        public IEnumerator GetEnumerator() => children.GetEnumerator();






    }
}