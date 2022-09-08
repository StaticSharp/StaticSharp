using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp.Gears{

    [ConstructorJs]
    public partial class Inline : BaseModifier, IInline {
        public Inline(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) {
        }
        public Inline(Hierarchical other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) {
        }



        protected virtual Task<Tag?> GenerateInlineHtmlInternalAsync(Context context, Tag elementTag) {
            return Task.FromResult<Tag?>(null);
        }
        public virtual async Task<Tag> GenerateInlineHtmlAsync(Context context, string? id = null) {

            await AddRequiredInclues(context);


            var tag = new Tag(TagName, id) { };


            tag.Add(await CreateConstructorScriptAsync(context));


            tag.Add(await GenerateInlineHtmlInternalAsync(context, tag));
            //tag.Add(After());

            return tag;
        }


    }


}