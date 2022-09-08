using StaticSharp.Gears;
using StaticSharp.Html;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    [System.Diagnostics.DebuggerNonUserCode]
    public class LinkJs : BaseModifierJs {
        public string HRef => NotEvaluatableValue<string>();
        public bool NewTab => NotEvaluatableValue<bool>();
    }

    public class LinkBindings<FinalJs> : BaseModifierBindings<FinalJs> where FinalJs : new() {
        public Binding<string> HRef { set { Apply(value); } }
        public Binding<bool> NewTab { set { Apply(value); } }
    }

    [Mix(typeof(LinkBindings<LinkJs>))]
    [ConstructorJs]
    public partial class Link : Inline {
        protected override string TagName => "a";

        public Inlines Children { get; } = new();

        public Link([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
        }
        public Link(Link other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) {
        }

        protected override async Task<Tag?> GenerateInlineHtmlInternalAsync(Context context, Tag elementTag) {
            return new Tag() {
                await Children.Select(x=> x.Value.GenerateInlineHtmlAsync(context,x.Key)).SequentialOrParallel(),
            };
        }

    }
}