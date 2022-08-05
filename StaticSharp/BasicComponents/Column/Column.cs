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

        protected BlockList children { get; } = new();

        public Column(Column<Js> other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {
            children = new(other.children);        
        }
        public Column(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

        public Column<Js> Children => this;
        public void Add(string? id, IBlock? value) {
            if (value != null) {
                children.Add(value,id);
            }
        }
        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }
    }

    [ScriptBefore]
    [ScriptAfter]
    public sealed class Column : Column<Symbolic.ColumnJs> {
        public override string TagName => "column";
        public Column(Column other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) { }
        public Column([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        //Пока невозможно использовать приведение из InterpolatedStringHandler из-за ошибок компилятора.
        /*public static implicit operator Column(WeekCollection weekCollection) {
            return new Column();
        }*/

        public override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return new Tag() {
                await children.Select(x=> x.Value.GenerateHtmlAsync(context,x.Key)).SequentialOrParallel(),
            };
        }
    }
}