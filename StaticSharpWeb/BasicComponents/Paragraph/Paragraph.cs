using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    [InterpolatedStringHandler]
    public class Paragraph : Block<Symbolic.Block> {

        public Paragraph(
            int literalLength,
            int formattedCount,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {        
        }

        public void AppendLiteral(string value,
            [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            Add(new Text(value, true, callerFilePath, callerLineNumber));
        }

        public void AppendFormatted(string value,
            [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            Add(new Text(value, false, callerFilePath, callerLineNumber));
        }
        public void AppendFormatted<T>(T t) {
            Console.WriteLine($"\tAppendFormatted called: {{{t}}} is of type {typeof(T)}");
        }


        public override Task<Tag> GenerateHtmlAsync(Context context) {
            throw new System.NotImplementedException();
        }
    }
}
