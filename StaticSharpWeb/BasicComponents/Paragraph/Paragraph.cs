using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    [ScriptBefore]
    [ScriptAfter]
    [InterpolatedStringHandler]
    public class Paragraph : Block<BlockJs>, IVoidEnumerable, IInline {

        protected List<IInline> children { get; } = new();
        public Paragraph Children => this;
        private new Binding<float> Height => default; //hide

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
            //Console.WriteLine($"\tAppendFormatted called: {{{t}}} is of type {typeof(T)}");
        }

        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }

        public override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return new Tag("div") {
                    await children.Select(x=>x.GenerateInlineHtmlAsync(context)).SequentialOrParallel()
                    //await Task.WhenAll(children.Select(x=>x.GenerateInlineHtmlAsync(context)))
                };
        }

        /*public override async Task<Tag> GenerateHtmlAsync(Context context, string? id) {
            AddRequiredInclues(context.Includes);
            return new Tag("div", id) {
                CreateScriptBefore(),
                ,
                CreateScriptAfter()
            };
        }*/

        async Task<Tag> IInline.GenerateInlineHtmlAsync(Context context) {
            return new Tag() {
                await Task.WhenAll(children.Select(x=>x.GenerateInlineHtmlAsync(context)))
            };
        }
    }
}
