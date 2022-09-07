using StaticSharp.Gears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StaticSharp {


    [InterpolatedStringHandler]
    public class Inlines : List<KeyValuePair<string?, IInline>>, IInlineCollector {
        public Inlines() : base() { }
        public Inlines(Inlines other) : base(other) { }

        public IEnumerable<IInline> Values => this.Select(x => x.Value);
        public void Add(string? id, IInline? value) {
            if (value != null) {
                //base.Add(id, block);
                Add(new KeyValuePair<string?, IInline>(id, value));
            }
        }

        public Inlines(
            int literalLength,
            int formattedCount){
        }

        /*public Inlines(
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {
        }*/


        public void AppendLiteral(string value, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            this.Add(new Text(value, true, callerFilePath, callerLineNumber));
        }
        /*public void AppendFormatted(string value, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            this.Add(new Text(value, false, callerFilePath, callerLineNumber));
        }*/

        public void AppendFormatted(Inlines values, string? format = null) {
            foreach (var value in values) {
                AppendFormatted(value.Value, value.Key);
            }
        }
        public void AppendFormatted(IInline value, string? format = null) {
            string? id = null;
            if (format != null) {
                if (format.StartsWith("##")) {
                    format = format.Substring(1);
                } else {
                    if (format.StartsWith("#")) {
                        var separatorSpacePosition = format.IndexOf(' ');
                        if (separatorSpacePosition > 0) {
                            id = format.Substring(1, separatorSpacePosition - 1);
                            format = format.Substring(separatorSpacePosition + 1);
                        } else {
                            id = format;
                            format = null;
                        }
                    }
                }
            }
            this.Add(id, value);
        }


        public void AppendFormatted<T>(T t) where T : struct {
            //TODO: inplement
            Console.WriteLine($"\tAppendFormatted called: {{{t}}} is of type {typeof(T)}");
        }



    }

}