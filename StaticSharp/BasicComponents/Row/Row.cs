using StaticSharp.Gears;
using StaticSharp.Html;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {



    [RelatedScript]
    public class Row : Block {
        protected override string TagName => "row";
        public Blocks Children { get; } = new();
        /*public void Add(string? id, IBlock? value) {
            if (value!=null)
                children.Add(id, value);
        }*/

        public static Space DefaultSpace = new Space();

        public Row([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        protected override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return new Tag() {
                await Children.Select(x=> x.Value.GenerateHtmlAsync(context,x.Key)).SequentialOrParallel(),
            };
        }
    }
}
