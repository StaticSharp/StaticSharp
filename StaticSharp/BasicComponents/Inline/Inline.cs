using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp{


    namespace Js {
        public class Inline : Hierarchical {

        }
    }

    namespace Gears {
        public class InlineBindings<FinalJs> : BaseModifierBindings<FinalJs> where FinalJs : new() {
        
        }
    }




    [ConstructorJs]
    public partial class Inline : BaseModifier, IInline {
        public Inline(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) {
        }
        public Inline(Inline other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) {
        }

        protected virtual Task ModifyHtmlAsync(Context context, Tag elementTag) {
            return Task.CompletedTask;
        }
        public virtual async Task<Tag> GenerateInlineHtmlAsync(Context context) {

            await AddRequiredInclues(context);


            var tag = new Tag(TagName) { };

            AddSourceCodeNavigationData(tag, context);

            tag.Add(await CreateConstructorScriptAsync(context));
            await ModifyHtmlAsync(context, tag);


            return tag;
        }


    }


}