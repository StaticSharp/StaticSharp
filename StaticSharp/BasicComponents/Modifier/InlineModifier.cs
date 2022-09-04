using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {
    public sealed class InlineModifier : BaseModifier, IInline, IInlineCollector {

        private List<KeyValuePair<string?, IInline>> children { get; } = new();
        public InlineModifier Children => this;
        public void Add(string? id, IInline? value) {
            if (value != null) {
                children.Add(new KeyValuePair<string?, IInline>(id, value));
            }
        }

        public InlineModifier([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        public async Task<Tag> GenerateInlineHtmlAsync(Context context, string? id) {
            await AddRequiredInclues(context);
            context = ModifyContext(context);

            var result = new Tag("span") {
                await CreateConstructorScriptAsync(context)
            };
            ModifyTag(result);

            foreach (var child in children) {
                result.Add(await child.Value.GenerateInlineHtmlAsync(context,child.Key));
            }
            return result;
        }

        
    }
}