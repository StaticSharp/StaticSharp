using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    [Mix(typeof(CheckboxJs))]
    public class CheckboxInlineJs {
    }



    [Mix(typeof(CheckboxBindings<CheckboxInlineJs>))]
    [ConstructorJs("Checkbox")]

    [ConstructorJs]
    public partial class CheckboxInline : Inline {
        protected override string TagName => "checkboxInline";
        public CheckboxInline(CheckboxInline other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) { }
        public CheckboxInline([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        protected override Task<Tag?> GenerateInlineHtmlInternalAsync(Context context, Tag elementTag) {
            return Task.FromResult<Tag?>(new Tag("input") {
                ["type"] = "checkbox"
            });
        }

    }






}