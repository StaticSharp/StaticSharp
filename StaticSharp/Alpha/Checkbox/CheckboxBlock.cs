using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    [Mix(typeof(CheckboxJs))]
    [Mix(typeof(BlockJs))]
    public partial class CheckboxBlockJs {
    }





    [Mix(typeof(CheckboxBindings<CheckboxBlockJs>))]
    [Mix(typeof(BlockBindings<CheckboxBlockJs>))]

    [ConstructorJs("Checkbox")]
    [ConstructorJs]
    public partial class CheckboxBlock : Block {

        protected override string TagName => "checkboxBlock";
        public CheckboxBlock(CheckboxBlock other, string callerFilePath, int callerLineNumber): base(other, callerFilePath, callerLineNumber) { }
        public CheckboxBlock([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        protected override Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {

            return Task.FromResult<Tag?>(new Tag("input") {
                ["type"] = "checkbox"
            });
        }        
    }






}