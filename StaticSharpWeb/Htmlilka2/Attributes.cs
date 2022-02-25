using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticSharpWeb.Html {

    public class Attributes : Dictionary<string, string> {

        private static IDictionary<string, string> ObjectToDictionary(object attributes) {

            Dictionary<string, string> result = new ();

            void Add(string key, object? value) {
                var valueString = value?.ToString();
                if (valueString != null) { 
                    result.Add(key, valueString);
                }
            }

            if (attributes == null || Equals(attributes, new { }))
                return result;

            if (attributes is IDictionary<string, object> dictionary) {
                foreach (var i in dictionary) { 
                    Add(i.Key, i.Value);
                }
                return result;
            }

            foreach (var i in attributes.GetType().GetProperties()) {
                Add(i.Name, i.GetValue(attributes));
            }
            return result;
        }

        public Attributes(object attributes) : base(ObjectToDictionary(attributes)) { }

        public void BuildAttributeString(StringBuilder builder) {
            foreach(var attribute in this) {
                if(string.IsNullOrEmpty(attribute.Value)) {
                    builder.Append($" {attribute.Key}");
                } else {
                    builder.Append($" {attribute.Key}=\"{attribute.Value.ReplaceInvalidAttributeValueSymbols()}\"");
                }

            }
        }

        public override string ToString() {
            var result = string.Empty;
            foreach(var attribute in this) {
                result += string.IsNullOrEmpty(attribute.Value) 
                    ? $" {attribute.Key}" 
                    : $" {attribute.Key}=\"{attribute.Value.ReplaceInvalidAttributeValueSymbols()}\"";
            }
            return result;
        }
    }
}