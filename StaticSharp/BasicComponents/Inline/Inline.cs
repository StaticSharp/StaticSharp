using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp.Gears{


    [System.Diagnostics.DebuggerNonUserCode]
    public class InlineJs : HierarchicalJs {

    }

    namespace Gears {
        public class InlineBindings<FinalJs> : BaseModifierBindings<FinalJs> where FinalJs : new() {
        
        }
    }




    [ConstructorJs]
    public partial class Inline : BaseModifier, IInline {
        public Inline(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) {
        }
        public Inline(Hierarchical other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) {
        }

        protected virtual Task<Tag?> GenerateInlineHtmlInternalAsync(Context context, Tag elementTag, string? format) {
            return Task.FromResult<Tag?>(null);
        }
        public virtual async Task<Tag> GenerateInlineHtmlAsync(Context context, string? id, string? format) {

            await AddRequiredInclues(context);


            var tag = new Tag(TagName, id) { };

            AddSourceCodeNavigationData(tag, context);

            tag.Add(await CreateConstructorScriptAsync(context));

            tag.Add(await GenerateInlineHtmlInternalAsync(context, tag, format));
            //tag.Add(After());

            return tag;
        }


    }


}