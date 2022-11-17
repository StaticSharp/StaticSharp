using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
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

        public Inlines Children { get; init; } = new();


        protected Inline(Inline other,
            int callerLineNumber = 0,
            string callerFilePath = ""
            ) : base(other, callerFilePath, callerLineNumber) {
            Children = new(other.Children);
        }
        public Inline(
            int callerLineNumber = 0,
            string callerFilePath = "") : base(callerFilePath, callerLineNumber) { }


        public virtual Task<string> GetPlaneTextAsync(Context context) => Task.FromResult("");


        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {
            foreach (var c in Children) {
                var childTag = await c.Value.GenerateHtmlAsync(context, new Role(true, c.Key));
                elementTag.Add(childTag);
            }
            await base.ModifyHtmlAsync(context, elementTag);
        }


        /*public virtual async Task<Tag> GenerateHtmlAsync(Context context) {

            await AddRequiredInclues(context);


            var tag = new Tag(TagName) { };

            AddSourceCodeNavigationData(tag, context);

            tag.Add(await CreateConstructorScriptAsync(context));
            await ModifyHtmlAsync(context, tag);


            return tag;
        }*/


    }


}