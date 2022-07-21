using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    [System.Diagnostics.DebuggerNonUserCode]
    public class SliderJs : BlockJs {
        public float Min =>     throw new NotEvaluatableException();
        public float Max =>     throw new NotEvaluatableException();
        public float Step =>    throw new NotEvaluatableException();
        public float Value =>   throw new NotEvaluatableException();//new InvalidUsageException();//
    }
    


    public abstract class Slider<Js> : Block<Js> where Js : SliderJs, new() {

        public Slider(Slider<Js> other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) { }

        public Slider(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(ThisFilePathWithNewExtension("js")));
        }

    }

    [ScriptBefore][ScriptAfter]
    public sealed class Slider : Slider<SliderJs> {
        public Binding<float> Min   { set; private get; }
        public Binding<float> Max   { set; private get; }
        public Binding<float> Step  { set; private get; }
        public Binding<float> Value { set; private get; }

        public Slider(Slider other, string callerFilePath, int callerLineNumber)
            : base(other, callerFilePath, callerLineNumber) {

            Min = other.Min;
            Max = other.Max;
            Step = other.Step;
            Value = other.Value;
        }
        public Slider([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        public override string TagName => "slider";

        public override Task<Tag> GenerateHtmlInternalAsync(Context context, Tag elementTag) {

            context.Includes.Require(new Style(AbsolutePath("Slider.scss")));

            return Task.FromResult(
                new Tag("input") {
                    ["type"] = "range"
                }
                );
        }
    }
}