using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Threading.Tasks;

namespace StaticSharp {
    public class Word : Element {

        public string Text { get; }

        public Word(string text, string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) {
            Text = text;
        }        

        public override async Task<Tag> GenerateHtmlAsync(Context context) {
            
            var measurer = context.TextMeasurer;


            return new Tag("w", new { 
                w = measurer.Measure(Text)
            }) {
                Text
            };
        }
    }
}
