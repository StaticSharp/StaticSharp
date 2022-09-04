using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    [RelatedScript]
    public class Column : Block, IBlockCollector {
        protected override string TagName => "column";
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

        protected override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return new Tag() {
                await Children.Select(x=> x.Value.GenerateHtmlAsync(context,x.Key)).SequentialOrParallel(),
            };
        }

    }

}