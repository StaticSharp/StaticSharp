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
            Console.WriteLine("begin");
            var measurer = await context.Font.CreateOrGetCached().CreateTextMeasurer(context.FontSize);
            Console.WriteLine("end");

            return new Tag("w", new { 
                w = measurer.Measure(Text)
            }) {
                Text
            };
        }
    }
}
