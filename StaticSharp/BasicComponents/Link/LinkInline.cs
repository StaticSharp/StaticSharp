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


    [Mix(typeof(LinkBindings<Js.LinkInline>))]
    [Mix(typeof(InlineBindings<Js.LinkInline>))]
    [ConstructorJs("Link")]
    [ConstructorJs]
    public partial class LinkInline : Inline {
        protected override string TagName => "a";

        Tree.INode? Node;
        public Inlines Children { get; } = new();


        public LinkInline(string url, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            HRef = url;
        }

        public LinkInline(Tree.INode node, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Node = node;
        }
        public LinkInline([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
        }
        public LinkInline(LinkInline other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) {
            Node = other.Node;
        }
        protected override async IAsyncEnumerable<KeyValuePair<string, string>> GetGeneratedBundingsAsync(Context context) {
            await foreach (var i in base.GetGeneratedBundingsAsync(context)) { yield return i; }
            if (Node != null) {
                var url = context.NodeToUrl(Node);
                yield return new("HRef", $"\"{url}\"");
            }
        }
        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {
            
            foreach (var i in Children) {
                var child = await i.Value.GenerateHtmlAsync(context);
                if (i.Modifier != null)
                    await i.Modifier.Apply(child);
                //child.AddAsChild();
                elementTag.Add(child);
            }


            /*return new Tag() {
                await Children.Select(x=> x.Value.GenerateInlineHtmlAsync(context, x.Id, x.Format)).SequentialOrParallel(),
            };*/
        }
    }


}