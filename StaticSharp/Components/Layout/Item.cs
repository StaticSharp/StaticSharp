
using StaticSharpWeb.Html;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    /*public abstract class Item : Component , IContainerConstraintsNone {

        protected virtual string TagName => "div";
        public virtual IEnumerable<Task<Tag>> Before(Context context) {
            yield return Task.FromResult(new JSCall(AbsolutePath("Item.js"), null, "Before").Generate(context));
        }
        public abstract Task<Tag> Content(Context context);
        public virtual IEnumerable<Task<Tag>> After(Context context) {
            yield return Task.FromResult(new JSCall(AbsolutePath("Item.js"), null, "After").Generate(context));
        }

        public override async Task<Tag> GenerateHtmlAsync(Context context) {
            var before = Task.WhenAll(Before(context));
            var content = Content(context);
            var after = Task.WhenAll(After(context));

            return new Tag(TagName) {
                await before,
                await content,
                await after
            };
        }
    }*/
}
