using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

 
    public class SliderJs : BlockJs {
        public NumberJs Min => new($"{value}.Min");
        public NumberJs Max => new($"{value}.Max");
        public NumberJs Step => new($"{value}.Step");
        public NumberJs Value => new($"{value}.Value");
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
        public Binding<NumberJs> Min { set; protected get; } = null!;
        public Binding<NumberJs> Max { set; protected get; } = null!;
        public Binding<NumberJs> Step { set; protected get; } = null!;
        public Binding<NumberJs> Value { set; protected get; } = null!;

        public Slider([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }

        public override async Task<Tag> GenerateHtmlAsync(Context context) {
            AddRequiredInclues(context.Includes);
            return new Tag("div") {
                CreateScriptBefore(),
                new Tag("input"){ 
                    ["type"] = "range"
                },
                CreateScriptAfter()                
                
            };
        }
    }

}