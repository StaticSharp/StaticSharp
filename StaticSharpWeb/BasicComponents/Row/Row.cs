using StaticSharp.Gears;
using StaticSharp.Html;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Symbolic {
        public class Row : Item {
            /*public RowJs(string value) : base(value) {
            }*/
        }
    }

    public abstract class Row<Js> : Item<Js>, IElementCollector<IElement> where Js : Symbolic.Row, new() {


        //record Row(
        Space? Space { get; set; } = null;
        float? GrowAbove { get; set; } = null;
        float? GrowBelow { get; set; } = null;

        protected List<IElement> children { get; } = new();
        public Row<Js> Children => this;
        public void AddElement(IElement value) {
            children.Add(value);
        }

        public static Space DefaultSpace = new Space();

        public Row(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

        public void AppendLiteral(string text, [CallerFilePath] string callerFilePath = "",[CallerLineNumber] int callerLineNumber = 0){

            TextProcessor.SplidText(text, children, callerFilePath, callerLineNumber);
            //Console.WriteLine($"\tAppended <{text}>");
        }

        public void AppendFormatted(IElement element /*,[CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0*/) {
            //Console.WriteLine($"\tAppendFormatted called: {{{t}}} is of type {typeof(T)}");
            //Console.WriteLine($"\tAppended the formatted object");
        }

        public void Add(string text) {
            //AppendLiteral(text);
        }

        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(AbsolutePath("Row.js")));
        }

    }

    [InterpolatedStringHandler]
    public sealed class Row : Row<Symbolic.Row> {
        public Row([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber)
            { }

        public Row(
            int literalLength,
            int formattedCount,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber)
            { }

        public override async Task<Tag> GenerateHtmlAsync(Context context) {
            AddRequiredInclues(context.Includes);
            return new Tag("row") {
                CreateScriptBefore(),
                await Task.WhenAll(children.Select(x=>x.GenerateHtmlAsync(context))),
                CreateScriptAfter()
            };
        }

    }
}
