using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    /*[ConstructorJs]
    public sealed  class Modifier : BaseModifier {
        public Modifier([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

    }*/


    /*[ScriptBefore]
    [ScriptAfter]
    public sealed class Modifier : BaseModifier, IBlock, IBlockCollector {

        private BlockList children { get; } = new();
        public Modifier Children => this;
        public void Add(string? id, IBlock? value) {
            if (value != null)
                children.Add(value, id);
        }

        public Modifier([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }


        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }



        public async Task<Tag> GenerateHtmlAsync(Context context, string? id) {
            
            return await GenerateHtmlWithChildrenAsync(
                context,
                id,
                (innerContext)=>children.Select(x=>x.Value.GenerateHtmlAsync(innerContext, x.Key))
            );
        }

    }*/
}