using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {
    public sealed class InlineModifier : BaseModifier, IInline, IInlineCollector {

        private List<IInline> children { get; } = new();
        public InlineModifier Children => this;
        public void Add(IInline? value) {
            if (value!=null)
                children.Add(value);
        }

        public InlineModifier([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        public async Task<Tag> GenerateInlineHtmlAsync(Context context) {
            /*(Tag result, context) = await GenerateHtmlAndContextAsync(context);
            foreach (var i in children) {
                result.Add(await i.GenerateInlineHtmlAsync(context));
            }
            return result;*/

            return await GenerateHtmlWithChildrenAsync(context, null, (innerContext) =>
                children.Select(x=>x.GenerateInlineHtmlAsync(innerContext))
            );
        }
    }
}