using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    public interface JScrollLayout : JBlock {
        public Block Child { get; }
        public double ScrollX { get; }
        public double ScrollY { get; }
        public double ScrollXActual { get; }
        public double ScrollYActual { get; }

        public double InternalWidth { get; }
        public double InternalHeight { get; }
    }

    namespace Gears {
        public class ScrollLayoutBindings<FinalJs> : BlockBindings<FinalJs> {
            public Binding<double> ScrollX { set { Apply(value); } }
            public Binding<double> ScrollY { set { Apply(value); } }
        }
    }

    [Mix(typeof(ScrollLayoutBindings<JScrollLayout>))]
    [ConstructorJs]
    public partial class ScrollLayout : Block {

        [Socket]
        public required Block Child { get; set; }
        public ScrollLayout(ScrollLayout other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
            Child = other.Child;
        }
        public ScrollLayout([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }

        public virtual ScrollLayout Assign(out Js.Variable<JScrollLayout> variable, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")// where T : Hierarchical where JsT : Js.Hierarchical 
        {
            variable = new(callerLineNumber, callerFilePath);
            return Assign(variable);
        }
        public ScrollLayout Assign(Js.Variable<JScrollLayout> variable) {
            if (VariableNames == null) VariableNames = new();
            VariableNames.Add(variable.Name);
            return this;
        }


        /*protected override void ModifyHtml(Context context, Tag elementTag) {

            elementTag.Add(CreateScript_SetCurrentSocket("Child"));
            elementTag.Add(Child.GenerateHtml(context));


            base.ModifyHtml(context, elementTag);
        }*/

    }

}