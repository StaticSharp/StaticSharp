using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsmlWeb.Html {

    public class Attributes : Dictionary<string, string> {

        private static IDictionary<string, string> ObjectToDictionary(object attributes) => 
            attributes != null && !Equals(attributes, new { })
                ? attributes.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(attributes).ToString())
                : new Dictionary<string, string>();


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