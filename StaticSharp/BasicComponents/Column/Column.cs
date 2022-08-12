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

    [ScriptBefore]
    [ScriptAfter]
    public class Column : Block, IBlockCollector {



        public override string TagName => "column";
        public Blocks Children { get; } = new();

        public Column(Column other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {
            Children = new(other.Children);        
        }
        public Column([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        //public Column Children => this;
        public void Add(string? id, IBlock? value) {
            if (value != null) {
                Children.Add(id, value);
            }
        }
        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }

        public override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return new Tag() {
                await Children.Select(x=> x.Value.GenerateHtmlAsync(context,x.Key)).SequentialOrParallel(),
            };
        }

    }

}