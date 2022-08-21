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
            await AddRequiredInclues(context);
            context = ModifyContext(context);

            var result = new Tag("span") {
                await CreateConstructorScriptAsync(context)
            };
            ModifyTag(result);

            foreach (var child in children) {
                result.Add(await child.GenerateInlineHtmlAsync(context));
            }
            return result;
        }
    }
}