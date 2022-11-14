using StaticSharp.Gears;

using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        [Mix(typeof(Link))]
        [Mix(typeof(Inline))]
        public partial class LinkInline {
        }
    }


    /*[Mix(typeof(LinkBindings<Js.LinkInline>))]
    [Mix(typeof(InlineBindings<Js.LinkInline>))]
    [ConstructorJs("Link")]
    [Mix(typeof(Link))]
    [ConstructorJs]
    public partial class LinkInline : Inline {
        protected override string TagName => "a";
        
        


        public LinkInline(string url,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") : base(callerFilePath, callerLineNumber) {
            //Url = url;
        }

        public LinkInline(Tree.Node node,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") : base(callerFilePath, callerLineNumber) {
            //Node = node;
        }
        public LinkInline(
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") : base(callerFilePath, callerLineNumber) {
        }
        public LinkInline(LinkInline other, int callerLineNumber, string callerFilePath) : base(other, callerFilePath, callerLineNumber) {
            //Node = other.Node;
        }

        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {

            //SetHref(context, elementTag);


            foreach (var i in Children) {
                var child = await i.Value.GenerateHtmlAsync(context,new Role(true,i.Key));
                elementTag.Add(child);
            }

        }

        public override Task<string> GetPlaneTextAsync(Context context) {
            return ((IPlainTextProvider)Children).GetPlaneTextAsync(context);
        }
    }*/


}