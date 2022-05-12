using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {
    public sealed class Body : BaseModifier {

        public Body([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        public async Task<Tag> GenerateHtmlWithChildrenAsync(Context context, Func<Context, IEnumerable<Task<Tag>>> children) {
            return await GenerateHtmlWithChildrenAsync(context, children, "body");
        }
/*        public async Task<Tag> GenerateHtmlAsync(Context context, IEnumerable<IElement> children) {
            //.Select(x=>x.GenerateHtmlAsync(context))
            return await GenerateHtmlAsync(context, children, "body");
        }*/
    }
}