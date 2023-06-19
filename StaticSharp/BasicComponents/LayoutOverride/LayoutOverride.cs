using StaticSharp.Gears;

namespace StaticSharp
{

    public interface JLayoutOverride : JBlock {
        public JBlock Child { get; }

        public double? OverrideX { get; set; }
        public double? OverrideY { get; set; }

        public double? OverrideWidth { get; set; }
        public double? OverrideHeight { get; set; }
    }

    [ConstructorJs]
    public partial class LayoutOverride: Block {
        [Socket]
        public required Block Child { get; set; }

        protected LayoutOverride(LayoutOverride other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) {
            Child = other.Child;
        }

    }
}
