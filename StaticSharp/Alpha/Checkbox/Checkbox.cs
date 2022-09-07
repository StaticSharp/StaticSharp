using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    [System.Diagnostics.DebuggerNonUserCode]
    public class MCheckboxJs : ObjectJs {
        public bool Enabled => NotEvaluatableValue<bool>();
        public bool Value => NotEvaluatableValue<bool>();
    }


    [Mix(typeof(MCheckboxJs))]
    [Mix(typeof(MBlockJs))]
    public partial class CheckboxBlockJs {
    }

    [Mix(typeof(MCheckboxJs))]
    public class CheckboxInlineJs {
    }



    public class MCheckboxBindings<FinalJs> : MBindings<FinalJs> where FinalJs : new() {
        public Binding<bool> Enabled { set { Apply(value); } }
        public Binding<bool> Value { set { Apply(value); } }
    }




    [Mix(typeof(MCheckboxBindings<CheckboxBlockJs>))]
    [Mix(typeof(MBlockBindings<CheckboxBlockJs>))]

    [RelatedScript("Checkbox")]
    public partial class CheckboxBlock : Block {

        protected override string TagName => "checkbox";
        public CheckboxBlock(CheckboxBlock other, string callerFilePath, int callerLineNumber): base(other, callerFilePath, callerLineNumber) { }
        public CheckboxBlock([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        protected override Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {

            return Task.FromResult<Tag?>(new Tag("input") {
                ["type"] = "checkbox"
            });
        }        
    }

    [Mix(typeof(MCheckboxBindings<CheckboxInlineJs>))]
    public partial class CheckboxInline : Inline {
        protected override string TagName => "checkbox";
        public CheckboxInline(CheckboxInline other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) { }
        public CheckboxInline([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        protected override Task<Tag?> GenerateInlineHtmlInternalAsync(Context context, Tag elementTag) {
            return Task.FromResult<Tag?>(new Tag("input") {
                ["type"] = "checkbox"
            });
        }

    }






}