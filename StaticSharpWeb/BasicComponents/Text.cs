using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    public class Text : Reactive {

        public string Value { get; }
        public bool Formatting { get; }

        public Text(string text, bool formatting = true, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {

            Value = text.Replace("\r\n", "\n").Replace("\r", "\n");
            Formatting = formatting;
        }        

        public override Task<Tag> GenerateHtmlAsync(Context context) {

            if (!Formatting) {
                return Task.FromResult(new Tag() { Value });
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
                    action(result);
                } else {
                    length++;
                }
            }
            if (length != 0)
                result.Add(Value.Substring(start, length));

            return Task.FromResult(result);

        }
    }
}
