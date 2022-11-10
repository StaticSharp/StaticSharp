using StaticSharp.Gears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Gears {
        public struct InlineIdFormatValue {
            public HtmlModifier? Modifier;
            public IInline Value;

            public InlineIdFormatValue(IInline value, HtmlModifier? modifier) {
                Modifier = modifier;
                Value = value;
            }
        }
    }

    [InterpolatedStringHandler]
    public class Inlines : List<InlineIdFormatValue> , IPlainTextProvider {
        public Inlines() : base() { }
        public Inlines(Inlines other) : base(other) { }

        public IEnumerable<IInline> Values => this.Select(x => x.Value);
        public void Add(IInline? value, HtmlModifier? modifier = null) {
            if (value != null) {
                //base.Add(id, block);
                Add(new InlineIdFormatValue(value, modifier));
            }
        }

        /*public void Add(IInline? value) {
            Add(value);
        }*/

        public void Add(
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0){

            Add(new Text(text, true, callerFilePath, callerLineNumber));
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
            Add(new Text(value, true, callerFilePath, callerLineNumber));
        }
        /*public void AppendFormatted(string value, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            this.Add(new Text(value, false, callerFilePath, callerLineNumber));
        }*/


        public void AppendFormatted(ValueTuple<string, IInline> value) {
            Add(value.Item2, new HtmlModifier().AssignParentProperty(value.Item1));
        }

        public void AppendFormatted(Inlines values) {
            foreach (var value in values) {
                Add(value.Value, value.Modifier);
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
            Add(value, null);
        }


        /*public void AppendFormatted<T>(T t) where T : struct {
            //TODO: inplement
            Console.WriteLine($"\tAppendFormatted called: {{{t}}} is of type {typeof(T)}");
        }*/



        //Link
        public void AppendFormatted(Tree.Node node, string? format = null, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            var link = new LinkInline(node, callerFilePath, callerLineNumber) {
                Children = {
                    (format != null)?format: (node.Representative?.Title ?? "")
                }
            };
            AppendFormatted(link);
        }
        public void AppendFormatted(string url, string format, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            var link = new LinkInline(url, callerFilePath, callerLineNumber) {
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