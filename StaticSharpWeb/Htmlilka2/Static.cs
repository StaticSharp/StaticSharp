using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticSharp.Html {
    public static class Static {
        private static readonly Dictionary<char, string> AttributeTagContentReplacement = new() { { '<', "&lt;" } };
        private static readonly char[] AttributeTagContentChars = AttributeTagContentReplacement.Keys.ToArray();

        public static string ReplaceInvalidTagContentSymbols(this string x) =>
            x.ReplaceSymbols(AttributeTagContentChars, AttributeTagContentReplacement);

        private static readonly Dictionary<char, string> AttributeValueReplacement = new() { { '"', "&quot;" } };
        private static readonly char[] AttributeValueChars = AttributeValueReplacement.Keys.ToArray();

        public static string ReplaceInvalidAttributeValueSymbols(this string x) =>
            x.ReplaceSymbols(AttributeValueChars, AttributeValueReplacement);

        private static string ReplaceSymbols(this string x, char[] keys, Dictionary<char, string> keyValuePairs) {
            int start = 0;
            int i = x.IndexOfAny(keys);
            if(i >= 0) {
                var stringBuilder = new StringBuilder(2 * x.Length);
                while(i >= 0) {
                    stringBuilder.Append(x, start, i - start);
                    stringBuilder.Append(keyValuePairs[x[i]]);
                    start = i + 1;
                    i = x.IndexOfAny(keys, start);
                }
                stringBuilder.Append(x, start, x.Length - start);
                return stringBuilder.ToString();
            }
            return x;
        }
    }
}