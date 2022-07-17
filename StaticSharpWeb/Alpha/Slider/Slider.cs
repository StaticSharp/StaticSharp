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
        public Slider(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

        /*public void Add(Row value) {
            if (value != null)
                children.Add(value);
        }*/

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

        public Slider([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        public override Task<Tag> GenerateHtmlChildrenAsync(Context context) {
            return Task.FromResult(
                new Tag("input") {
                    ["type"] = "range"
                }
                );
        }

        /*public override async Task<Tag> GenerateHtmlAsync(Context context, string? id = null) {
            AddRequiredInclues(context.Includes);
            return new Tag("div",id) {
                CreateScriptBefore(),
                new Tag("input"){ 
                    ["type"] = "range"
                },
                CreateScriptAfter()                
                
            };
        }*/
    }

}