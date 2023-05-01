using StaticSharp.Gears;

namespace StaticSharp {

    public interface JId : JModifier {
        public string Value { get; set; }
    }

    [ConstructorJs]
    public partial class Id : Modifier {
    }


}


