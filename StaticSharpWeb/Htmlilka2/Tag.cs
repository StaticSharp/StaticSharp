using StaticSharpGears;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace StaticSharp.Html {


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

        Dictionary<string, object>? style = null;
        public IDictionary<string, object> Style {
            get {
                if (style == null) {
                    style = new();
                }
                return style;
            }
            set {
                if (value == null) {
                    style = new();
                } else {
                    style = new Dictionary<string, object>(value);
                }
            }
        }

        public string? Id {
            get { 
                return this["id"]?.ToString();
            }
            set {
                if (string.IsNullOrEmpty(value)) value = null;
                this["id"] = value;
            }
        }

        Dictionary<string, object>? attributes = null;
        public IDictionary<string, object> Attributes {
            get {
                if (attributes == null) {
                    attributes = new();
                }
                return attributes;
            }
            set {
                if (value == null) {
                    attributes = new();
                } else {
                    attributes = new Dictionary<string, object>(value);
                }                
            }
        }

        public Tag Children => this;

        public Tag(string? name = null, string? id = null) {
            Name = name;
            if (!string.IsNullOrEmpty(id))
                Id = id;
        }


        public object? this[string attributeName] {            
            get {
                return Attributes.TryGetValue(attributeName, out var value) ? value?.ToString() : null;
            }
            set {
                if (value == null) {
                    Attributes.Remove(attributeName);
                } else {
                    Attributes[attributeName] = value;
                }
            }
        }

        

        /*public Tag(string? name, object? attributes = null) {
            if (attributes!=null)
                Attributes = SoftObject.ObjectToDictionary(attributes);
            Name = name;
        }*/

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


        public static string? AttributeValueToString(object? value) {
            if (value == null) return null;

            if (value is float valueAsFloat) {
                return valueAsFloat.ToString(CultureInfo.InvariantCulture);
            }

            if (value is double valueAsDouble) {
                return valueAsDouble.ToString(CultureInfo.InvariantCulture);
            }

            if (value is bool valueAsBool) {
                return valueAsBool.ToString().ToLower();
            }

            return value.ToString();
        }

        public virtual void WriteHtml(StringBuilder builder) {

            void WriteCssStyle(StringBuilder builder) {
                /*if (styleObject is string styleObjectString) {
                    builder.Append(styleObjectString);
                } else {
                    
                }*/
                /*var styleDictionary = SoftObject.ObjectToDictionary(styleObject);
                if (styleDictionary == null) return;
*/
                if (style == null) return;
                if (style.Count == 0) return;

                builder.Append($" style=\"");

                foreach (var item in style) {
                    string? stringValue;
                    if (item.Value is Color color) {
                        stringValue = ColorTranslator.ToHtml(color);
                    } else {
                        stringValue = item.Value.ToString();
                    }
                    if (stringValue == null) continue;

                    builder.Append($"{Gears.CaseConverter.PascalToKebabCase(item.Key)}:{stringValue};");
                }
                builder.Append($"\"");

            }

            void WriteAttributes(StringBuilder builder) {
                if (attributes == null) return;
                foreach (var i in attributes) {
                    if (i.Key.ToLower() == "style") {
                        throw new System.Exception("Do not set Style through attributes, use Style property instead.");
                    }

                    /*if (i.Key.ToLower() == "style") {

                        if (i.Value != null) {
                            builder.Append(" style = \"");
                            WriteCssStyle(builder, i.Value);
                            builder.Append("\"");
                        }
                    } else {

                    }*/
                    var valueString = AttributeValueToString(i.Value);
                    if (valueString != null) {
                        var kebebKey = Gears.CaseConverter.PascalToKebabCase(i.Key);
                        if (valueString.Length > 0) {
                            builder.Append($" {kebebKey}=\"{valueString.ReplaceInvalidAttributeValueSymbols()}\"");
                        } else {
                            builder.Append(' ').Append(kebebKey);
                        }
                    }

                }
            }



            if (Name != null) {
                builder.Append('<').Append(Name);
                WriteAttributes(builder);
                WriteCssStyle(builder);
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