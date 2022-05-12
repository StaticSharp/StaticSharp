using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Symbolic {
        public class ColumnJs : BlockJs {
            /*public ColumnJs(string value) : base(value) {
            }*/
        }
    }


    public abstract class Column<Js> : Block<Js>, IBlockCollector where Js : Symbolic.ColumnJs, new() {
        public Column(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

        public Space? Space { get; set; } = null;

        protected List<IBlock> children { get; } = new();
        public Column<Js> Children => this;
        public void Add(IBlock? value) {
            if (value != null) {
                children.Add(value);
            }
        }


        public static Space DefaultSpace = new Space() {
            MinBetween = 8
        };

        /*public void Add(Row value) {
            if (value != null)
                children.Add(value);
        }*/

        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }

    }

    [ScriptBefore]
    [ScriptAfter]
    public sealed class Column : Column<Symbolic.ColumnJs> {
        public Column([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }


        //Пока невозможно использовать приведение из InterpolatedStringHandler из-за ошибок компилятора.
        /*public static implicit operator Column(WeekCollection weekCollection) {
            return new Column();
        }*/


        public override async Task<Tag> GenerateHtmlAsync(Context context) {
            AddRequiredInclues(context.Includes);
            return new Tag("column") {
                CreateScriptBefore(),
                await Task.WhenAll(children.Select(x=>x.GenerateHtmlAsync(context))),
                CreateScriptAfter()
            };
        }
    }

}