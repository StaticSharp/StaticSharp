using StaticSharp.Gears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    namespace Gears {
        public struct InlineIdFormatValue {
            public string? Id;
            public string? Format;
            public IInline Value;

            public InlineIdFormatValue(string? id, string? format, IInline value) {
                Id = id;
                Format = format;
                Value = value;
            }
        }
    }

    [InterpolatedStringHandler]
    public class Inlines : List<InlineIdFormatValue> {
        public Inlines() : base() { }
        public Inlines(Inlines other) : base(other) { }

        public IEnumerable<IInline> Values => this.Select(x => x.Value);
        public void Add(string? id, string? format, IInline? value) {
            if (value != null) {
                //base.Add(id, block);
                Add(new InlineIdFormatValue(id, format, value));
            }
        }

        public void Add(IInline? value) {
            Add(null, null, value);
        }

        public void Add(
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0){

            Add(null,null,new Text(text, true, callerFilePath, callerLineNumber));
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
                Add(value.Id, value.Format, value.Value);
                //AppendFormatted(value.Value, value.Key);
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
                            id = format.Substring(1);
                            format = null;
                        }
                    }
                }
            }
            this.Add(id, format, value);
        }


        /*public void AppendFormatted<T>(T t) where T : struct {
            //TODO: inplement
            Console.WriteLine($"\tAppendFormatted called: {{{t}}} is of type {typeof(T)}");
        }*/


        public void AppendFormatted(StaticSharpEngine.ITypedRepresentativeProvider<Page> node, string? format = null, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            var link = new NodeLink(node, callerFilePath, callerLineNumber) {
                Children = {
                    (format != null)?format:node.Representative.Title
                }
            };
            AppendFormatted(link);
        }




    }

}