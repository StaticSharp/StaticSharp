using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {
    public interface JButton : JModifier {
        bool SetCursor { get; set; }
    }


    [ConstructorJs]
    public partial class Button : Modifier {

        public string Script { get; set; }

        public override IdAndScript Generate(string elementId, Context context) {
            var result = base.Generate(elementId, context);
            result.Script.Add($"{result.Id}.Script = function(e){{{Script}}}");
            return result;
        }
    }

    


}


