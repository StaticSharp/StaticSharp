using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    [System.Diagnostics.DebuggerNonUserCode]
    public class CheckboxJs : BlockJs {
        public bool Enabled =>     NotEvaluatableValue<bool>();
        public bool Value =>   NotEvaluatableValue<bool>();
    }


    public class CheckboxBindings<FinalJs> : BlockBindings<FinalJs> where FinalJs : new() {
        public CheckboxBindings(Dictionary<string, string> properties) : base(properties) {}
        public Expression<Func<FinalJs, bool>> Enabled { set { AssignProperty(value); } }
        public Expression<Func<FinalJs, bool>> Value { set { AssignProperty(value); } }
    }





    [RelatedScript]
    public class Checkbox : Block, IInline {

        public new SliderBindings<CheckboxJs> Bindings => new(Properties);
        protected override string TagName => "checkbox";


        public Checkbox(Checkbox other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) { }

        public Checkbox([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        /*public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }*/

        protected override Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {

            context.Includes.Require(new Style(AbsolutePath("Slider.scss")));

            return Task.FromResult<Tag?>(new Tag("input") {
                    ["type"] = "range"
                });
        }

        public async Task<Tag> GenerateInlineHtmlAsync(Context context, string? id) {
            return new Tag("input", id) {
                ["type"] = "checkbox",

            };
        }
    }

    
    /*public sealed class Slider : Slider<SliderJs> {
        

        public Slider(Slider other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {

            Min = other.Min;
            Max = other.Max;
            Step = other.Step;
            Value = other.Value;
        }
        public Slider([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        

        
    }*/
}