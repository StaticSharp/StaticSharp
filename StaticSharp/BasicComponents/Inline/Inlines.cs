using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using StaticSharp.Core;

namespace StaticSharp
{

    [InterpolatedStringHandler]
    public class Inlines : IPlainTextProvider, IEnumerable<IInline> {


        private List<IInline>? items;
        public Inlines() { }
        public Inlines(Inlines other) {
            items = other.items;
        }
        public void Add(IInline? value) {
            if (value != null) {
                if (items == null)
                    items = new();
                items.Add(value);
            }
        }



        public Inlines(
            int literalLength,
            int formattedCount){
        }

        public static implicit operator Inlines(string text) => new Inlines { text };

        public void AppendLiteral(string value, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            Add(new Text(value, callerLineNumber, callerFilePath));
        }
        public void Add(
            string text,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") {

            Add(new Text(text, callerLineNumber, callerFilePath));
        }


        public void AppendFormatted(IInline value) {
            Add(value);
        }


        /*public void AppendFormatted(ValueTuple<string, IInline> value) {
            Add(value.Item1, value.Item2);
        }*/


        public void AppendFormatted(Inlines values) {
            foreach (var value in values)
                Add(value);            
        }
        public void Add(Inlines? value) {
            if (value != null) AppendFormatted(value);
        }



        public void AppendFormatted<T>(T t, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") where T : struct {
            Add(t.ToString(), callerLineNumber, callerFilePath);
        }
        public void AppendFormatted(string t, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
            Add(t, callerLineNumber, callerFilePath);
        }


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
        public void AppendFormatted(Uri url, string format, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            var link = new Inline(callerLineNumber, callerFilePath) {
                ExternalLink = url.ToString(),
                Children = {
                    format
                }
            };
            AppendFormatted(link);
        }



        public string GetPlainText(Context context) {
            var result = new StringBuilder();
            foreach (var i in this) {
                result.Append(i.GetPlainText(context));                
            }
            return result.ToString();
        }



        IEnumerator<IInline> IEnumerable<IInline>.GetEnumerator() {
            if (items != null)
                return items.GetEnumerator();
            return Enumerable.Empty<IInline>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (this as IEnumerable<IInline>).GetEnumerator();
        }


    }

}