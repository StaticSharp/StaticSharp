using NUglify.Helpers;
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections;
using System.Runtime.CompilerServices;

namespace StaticSharp {




    public class Blocks : IEnumerable<Block> {

        private List<Block>? items;
        public Blocks() { }
        public Blocks(Blocks other){
            items = other.items;
        }
        

        IEnumerator<Block> IEnumerable<Block>.GetEnumerator() {
            if (items != null)
                return items.GetEnumerator();
            return Enumerable.Empty<Block>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (this as IEnumerable<Block>).GetEnumerator();
        }


        public void Add(Block? value) {
            if (value != null) {
                if (items == null)
                    items = new();
                items.Add(value);
            }
        }


        public void Add(IEnumerable<string> texts,
            [CallerFilePath] string callerFilePath = "",[CallerLineNumber] int callerLineNumber = 0){
            foreach (var i in texts) {
                Add(new Paragraph(i, callerLineNumber, callerFilePath));
            }
        }

        public void Add(IEnumerable<Block?>? values) => values?.ForEach(Add);

        public void Add(
            Inlines? inlines,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0){
            if (inlines != null)
                Add(new Paragraph(inlines, callerLineNumber, callerFilePath));
        }

        public void Add(
            Inline? inline,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0){
            if (inline != null)
                Add(new Paragraph(inline, callerLineNumber, callerFilePath));
        }

        public void Add(
            string? text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0){
            if (text != null)
                Add(new Paragraph(text, callerLineNumber, callerFilePath));
        }



    }

}