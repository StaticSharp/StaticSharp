using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {
    public interface JMaterialShadow : JAbstractBoxShadow {
        public double Elevation  { get; set; }
    }

    [ConstructorJs]
    public partial class MaterialShadow : AbstractBoxShadow {

    }
}
