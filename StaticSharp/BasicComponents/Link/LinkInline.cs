using StaticSharp.Gears;

using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        [Mix(typeof(Js.Link))]
        [Mix(typeof(Js.Inline))]
        public partial class LinkInline {
        }
    }


    [Mix(typeof(LinkBindings<Js.LinkInline>))]
    [Mix(typeof(InlineBindings<Js.LinkInline>))]
    [ConstructorJs("Link")]
    [ConstructorJs]
    public partial class LinkInline : Inline {
        protected override string TagName => "a";
        
        StaticSharpEngine.INode? Node;
        public Inlines Children { get; } = new();


        public LinkInline(string url, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            HRef = url;
        }

        public LinkInline(StaticSharpEngine.INode node, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Node = node;
        }
        public LinkInline([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
        }
        public LinkInline(LinkInline other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) {
            Node = other.Node;
        }
        public override async IAsyncEnumerable<KeyValuePair<string, string>> GetGeneratedBundingsAsync(Context context) {
            await foreach (var i in base.GetGeneratedBundingsAsync(context)) { yield return i; }
            if (Node != null) {
                var url = context.NodeToUrl(Node);
                yield return new("HRef", $"\"{url}\"");
            }
        }
        protected override async Task<Tag?> GenerateInlineHtmlInternalAsync(Context context, Tag elementTag, string? format) {
            return new Tag() {
                await Children.Select(x=> x.Value.GenerateInlineHtmlAsync(context, x.Id, x.Format)).SequentialOrParallel(),
            };
        }
    }


}