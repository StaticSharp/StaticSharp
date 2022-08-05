using StaticSharp.Gears;
using StaticSharp.Html;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    public class RowJs : BlockJs {
        /*public RowJs(string value) : base(value) {
        }*/
    }
    


    public abstract class Row<Js> : Block<Js>, IBlockCollector where Js : RowJs, new() {


        //record Row(
        Space? Space { get; set; } = null;
        float? GrowAbove { get; set; } = null;
        float? GrowBelow { get; set; } = null;

        protected BlockList children { get; } = new();
        public Row<Js> Children => this;
        public void Add(string? id, IBlock? value) {
            if (value!=null)
                children.Add(value,id);
        }

        public static Space DefaultSpace = new Space();

        public Row(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }



        /*public void AppendLiteral(string text, [CallerFilePath] string callerFilePath = "",[CallerLineNumber] int callerLineNumber = 0){

            TextProcessor.SplidText(text, children, callerFilePath, callerLineNumber);
            //Console.WriteLine($"\tAppended <{text}>");
        }

        public void AppendFormatted(IElement element *//*,[CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0*//*) {
            //Console.WriteLine($"\tAppendFormatted called: {{{t}}} is of type {typeof(T)}");
            //Console.WriteLine($"\tAppended the formatted object");
        }

        public void Add(string text) {
            //AppendLiteral(text);
        }*/

        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }

    }

    [ScriptBefore]
    [ScriptAfter]
    public sealed class Row : Row<RowJs> {

        public override string TagName => "row";

        public Row([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber)
            { }

        /*public Row(
            int literalLength,
            int formattedCount,
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber)
            { }*/

        public override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return new Tag() {
                await children.Select(x=> x.Value.GenerateHtmlAsync(context,x.Key)).SequentialOrParallel(),
            };
        }

    }
}
