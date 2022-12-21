using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    public class Text : CallerInfo, IInline {

        public string Value { get; set; }
        public bool Formatting { get; }

        public Text(string text, bool formatting = true, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) {

            Value = text.Replace("\r\n", "\n").Replace("\r", "\n");
            Formatting = formatting;
        }        

        public Tag GenerateHtml(Context context, Role? role) {
            
            var chars = Value.ToPrintableChars();
            HashSet<string> families = new();
            foreach (var fontFamilyGenome in context.FontFamilies) {
                var fontFamily = fontFamilyGenome.Result;
                var font = fontFamily.FindFont(context.FontWeight, context.ItalicFont);
                var fontSubset = context.FontSubsets.GetOrAdd(font.Key, key => new FontSubset(font));

                var numChars = chars.Count;
                chars = fontSubset.AddChars(chars);
                if (chars.Count < numChars) {
                    families.Add(fontFamily.Name);
                    numChars = chars.Count;
                }
                if (numChars == 0)
                    break;
            }

            

            if (!Formatting) {
                return new Tag() { Value };
            }

            Dictionary<char, Action<Tag>> specialCharacters = new Dictionary<char, Action<Tag>>() {
                ['\n'] = x =>  x.Add(new Tag("br")),
            };

            var result = new Tag();
            int start = 0;
            int length = 0;

            for (int i = 0; i<Value.Length; i++ ) { 
                var c = Value[i];
                if (specialCharacters.TryGetValue(c, out var action)) {
                    if (length!=0)
                        result.Add(Value.Substring(start, length));
                    start = i+1;
                    length = 0;
                    action(result);
                } else {
                    length++;
                }
            }
            if (length != 0)
                result.Add(Value.Substring(start, length));

            return result;

        }

        public string GetPlaneText(Context context) {
            return Value;
        }
    }
}
