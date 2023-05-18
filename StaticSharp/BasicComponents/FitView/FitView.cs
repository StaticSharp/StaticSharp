using StaticSharp.Gears;


namespace StaticSharp {


    public interface JFitView : JAspectBlock {
        public JBlock Child { get; }
    }


    [ConstructorJs]
    public partial class FitView : AspectBlock {
        [Socket]
        public required Block Child { get; set; }
        public FitView(FitView other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
            Child = other.Child;
        }

    }

}