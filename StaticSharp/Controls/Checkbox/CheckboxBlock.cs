using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        [Mix(typeof(Checkbox))]
        [Mix(typeof(Block))]
        public partial class CheckboxBlock {
        }
    }


    [Mix(typeof(CheckboxBindings<Js.CheckboxBlock>))]
    [Mix(typeof(BlockBindings<Js.CheckboxBlock>))]

    [ConstructorJs("Checkbox")]
    [ConstructorJs]
    public partial class CheckboxBlock : Block {

        public CheckboxBlock(CheckboxBlock other, string callerFilePath, int callerLineNumber): base(other, callerFilePath, callerLineNumber) { }
        public CheckboxBlock([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {            
            elementTag.Add(
                new Tag("input") {
                    ["type"] = "checkbox"
                }
            );
            await base.ModifyHtmlAsync(context, elementTag);
        }        
    }






}