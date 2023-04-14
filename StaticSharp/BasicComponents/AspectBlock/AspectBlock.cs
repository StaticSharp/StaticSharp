using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;
namespace StaticSharp {
    public enum Fit { 
        Inside,
        Outside,
        Stretch
    }

    public interface JAspectBlock : JBlock {
        public double Aspect { get; set; }
        public Fit Fit { get; set; }
        public double GravityVertical { get; set; }
        public double GravityHorizontal { get; set; }
    }

    [ConstructorJs]
    public partial class AspectBlock : Block {
        protected static void SetNativeSize(Scopes.Group script, string elementVariableName, double width, double height) {
            script.Add($"{elementVariableName}.NativeWidth = {width}");
            script.Add($"{elementVariableName}.NativeHeight = {height}");
        }
    }
}