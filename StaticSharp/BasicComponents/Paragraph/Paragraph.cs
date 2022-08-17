using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    [RelatedScript]
    [InterpolatedStringHandler]
    public class Paragraph : Block, IVoidEnumerable, IInline {

        protected List<IInline> children { get; } = new();
        public Paragraph Children => this;


        /*public static implicit operator Paragraph(string text) {
            string callerFilePath = "";
            int callerLineNumber = 0;

            var paragraph = new Paragraph(callerFilePath, callerLineNumber);
            paragraph.AppendLiteral(text, callerFilePath, callerLineNumber);
            return paragraph;
        }*/

        public Paragraph(string text,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            AppendLiteral(text, callerFilePath, callerLineNumber);
        }

        public Paragraph(Paragraph other,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0) : base(other, callerFilePath, callerLineNumber) {

            children = new(other.children);
        }

        public Paragraph(
            int literalLength,
            int formattedCount,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {
        }

        public Paragraph(
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {
        }

        public void Add(IInline? value) {
            if (value != null) {
                children.Add(value);
            }
        }

        public void Add(string value,
            [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            Add(new Text(value, true, callerFilePath, callerLineNumber));
        }

        public void AppendLiteral(string value,
            [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            Add(new Text(value, true, callerFilePath, callerLineNumber));
        }

        public void AppendFormatted(string value,
            [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            Add(new Text(value, false, callerFilePath, callerLineNumber));
        }

        public void AppendFormatted(IInline value) {
            Add(value);
        }
        public void AppendFormatted<T>(T t) where T : struct {
            //TODO: inplement
            //Console.WriteLine($"\tAppendFormatted called: {{{t}}} is of type {typeof(T)}");
        }

        protected override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return new Tag("div") {
                    await children.Select(x=>x.GenerateInlineHtmlAsync(context)).SequentialOrParallel()
            };
        }

        async Task<Tag> IInline.GenerateInlineHtmlAsync(Context context) {
            return new Tag() {
                await Task.WhenAll(children.Select(x=>x.GenerateInlineHtmlAsync(context)))
            };
        }
    }
}
