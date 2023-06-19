using StaticSharp.Gears;

namespace StaticSharp
{

    public interface JMenu : JBlock {
        public JBlock Button { get; }

        public JBlock Popup { get; }
    }

    [RelatedStyle]
    [ConstructorJs]
    public partial class Menu : Block {
        [Socket]
        public required Block Button { get; set; }

        [Socket]
        public required Block Popup { get; set; }

        protected Menu(Menu other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) {
            Button = other.Button;
            Popup = other.Popup;
        }

    }
}
