using StaticSharp.Gears;

namespace StaticSharp {
    public enum Fit { 
        Inside,
        Outside,
        Stretch
    }

    public interface JAspectBlock : JBlock {
        public double NativeAspect { get; set; }
        public Fit Fit { get; set; }
        public double GravityVertical { get; set; }
        public double GravityHorizontal { get; set; }
    }

    [ConstructorJs]
    public abstract partial class AspectBlock : Block {
        /*protected static void SetNativeSize(Scopes.Group script, string elementVariableName, double width, double height) {
            script.Add($"{elementVariableName}.NativeWidth = {width.ToStringInvariant()}");
            script.Add($"{elementVariableName}.NativeHeight = {height.ToStringInvariant()}");
        }*/
    }
}