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
            ) : base(other, callerLineNumber, callerFilePath) {
            Children = new(other.Children);
        }
        public Inline(
            int callerLineNumber = 0,
            string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }


        public virtual string GetPlaneText(Context context) => "";


        protected override void ModifyHtml(Context context, Tag elementTag) {
            foreach (var c in Children) {
                var childTag = c.Value.GenerateHtml(context, new Role(true, c.Key));
                elementTag.Add(childTag);
            }
            base.ModifyHtml(context, elementTag);
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