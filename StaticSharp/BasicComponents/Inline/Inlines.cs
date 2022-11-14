using StaticSharp.Gears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {



    [InterpolatedStringHandler]
    public class Inlines : List<KeyValuePair<string?, IInline>> , IPlainTextProvider {
        public Inlines() : base() { }
        public Inlines(Inlines other) : base(other) { }

        public IEnumerable<IInline> Values => this.Select(x => x.Value);
        public void Add(string? propertyName, IInline? value) {
            if (value != null) {
                Add(new KeyValuePair<string?, IInline>(propertyName,value));
            }
        }

        /*public void Add(IInline? value) {
            Add(value);
        }*/

        public void Add(
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0){

            Add(null, new Text(text, true, callerFilePath, callerLineNumber));
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
            Add(null, new Text(value, true, callerFilePath, callerLineNumber));
        }
        /*public void AppendFormatted(string value, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            this.Add(new Text(value, false, callerFilePath, callerLineNumber));
        }*/


        public void AppendFormatted(ValueTuple<string, IInline> value) {
            Add(value.Item1, value.Item2);
        }

        public void AppendFormatted(Inlines values) {
            foreach (var value in values) {
                Add(value.Key, value.Value);
            }
        }
        public void AppendFormatted(IInline value) {
            /*string? id = null;
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
            }*/
            Add(null, value);
        }


        /*public void AppendFormatted<T>(T t) where T : struct {
            //TODO: inplement
            Console.WriteLine($"\tAppendFormatted called: {{{t}}} is of type {typeof(T)}");
        }*/



        //Link
        public void AppendFormatted(Tree.Node node, string? format = null, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            var link = new Inline(callerLineNumber, callerFilePath) {
                InternalLink = node,
                Children = {
                    (format != null)?format: (node.Representative?.Title ?? "")
                }
            };
            AppendFormatted(link);
        }
        public void AppendFormatted(string url, string format, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            var link = new Inline(callerLineNumber, callerFilePath) {
                ExternalLink = url,
                Children = {
                    format
                }
            };
            AppendFormatted(link);
        }




        public async Task<string> GetPlaneTextAsync(Context context) {
            var result = new StringBuilder();
            foreach (var i in this) {
                result.Append(await i.Value.GetPlaneTextAsync(context));                
            }
            return result.ToString();
        }
    }

}