
using StaticSharp.Gears;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace StaticSharp.Html {


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

        public void MakeIdFromContent() {
            Id = null;
            Id = Hash.CreateFromString(this.GetHtml()).ToString(16);
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
                        stringValue = color.ToJavascriptString();// ColorTranslator.ToHtml(color);
                    } else {
                        stringValue = item.Value.ToString();
                    }
                    if (stringValue == null) continue;
                    builder.Append($"{item.Key}:{stringValue};");
                    //builder.Append($"{Gears.CaseConverter.PascalToKebabCase(item.Key)}:{stringValue};");
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
                        //var kebebKey = Gears.CaseConverter.PascalToKebabCase(i.Key);
                        if (valueString.Length > 0) {
                            builder.Append($" {i.Key}=\"{valueString.ReplaceInvalidAttributeValueSymbols()}\"");
                        } else {
                            builder.Append(' ').Append(i.Key);
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



        public static Tag Meta(string property, string content) {
            return new Tag("meta") {
                ["property"] = property,
                ["content"] = content
            };
        }


    }
}