using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;

namespace StaticSharp {
    public interface JButton : JModifier {
        bool EventPropagation { get; set; }
    }


    [ConstructorJs]
    public partial class Button : Modifier {

        public string Script { get; set; }

        public override IdAndScript Generate(Tag element, Context context) {
            var result = base.Generate(element, context);
            result.Script.Add($"{result.Id}.Script = function(e){{{Script}}}");
            return result;
        }
    }

    


}


