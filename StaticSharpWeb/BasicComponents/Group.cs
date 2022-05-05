using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    [InterpolatedStringHandler]
    public class Group: Reactive, IElementCollector {

        public List<IElement> Children { get; } = new();

        public Group(
            int literalLength,
            int formattedCount,
            /*, IElementCollector collector*/
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {

        }

        public Group([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {
        
        }

        public void AppendLiteral(string value,
            [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            Add(new Text(value,true, callerFilePath, callerLineNumber));
        }

        public void AppendFormatted(string value,
            [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            Add(new Text(value,false, callerFilePath, callerLineNumber));
        }
        public void AppendFormatted<T>(T t) {
            Console.WriteLine($"\tAppendFormatted called: {{{t}}} is of type {typeof(T)}");
        }

        public void Add(IElement element) {
            if (element!=null)
                Children.Add(element);
        }

        

        public override Task<Tag> GenerateHtmlAsync(Context context) {
            throw new NotImplementedException();
        }
    }
}
