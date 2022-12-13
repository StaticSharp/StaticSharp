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
        public Inlines(
            int literalLength,
            int formattedCount){
        }

        public void AppendLiteral(string value, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            Add(null, new Text(value, true, callerLineNumber, callerFilePath));
        }
        public void Add(
            string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) {

            Add(null, new Text(text, true, callerLineNumber, callerFilePath));
        }


        public void AppendFormatted(IInline value) {
            Add(null, value);
        }
        public void Add(IInline? value) {
            if (value != null) {
                Add(new KeyValuePair<string?, IInline>(null, value));
            }
        }




        public void AppendFormatted(ValueTuple<string, IInline> value) {
            Add(value.Item1, value.Item2);
        }
        public void Add(string? propertyName, IInline? value) {
            if (value != null) {
                Add(new KeyValuePair<string?, IInline>(propertyName, value));
            }
        }




        public void AppendFormatted(Inlines values) {
            foreach (var value in values)
                Add(value.Key, value.Value);            
        }
        public void Add(Inlines? value) {
            if (value != null) AppendFormatted(value);
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



        public string GetPlaneText(Context context) {
            var result = new StringBuilder();
            foreach (var i in this) {
                result.Append(i.Value.GetPlaneText(context));                
            }
            return result.ToString();
        }
    }

}