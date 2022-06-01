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


        public Binding<float> ChildrenLayoutWidth { set; protected get; }

        public Column(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

        protected BlockList children { get; } = new();
        public Column<Js> Children => this;
        public void Add(string? id, IBlock? value) {
            if (value != null) {
                children.Add(value,id);
            }
        }


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


        public override async Task<Tag> GenerateHtmlAsync(Context context, string? id) {
            AddRequiredInclues(context.Includes);
            return new Tag("column", id) {
                CreateScriptBefore(),
                await children.Select(x=> x.Value.GenerateHtmlAsync(context,x.Key)).SequentialOrParallel(),
                CreateScriptAfter()
            } ;
        }
    }

}