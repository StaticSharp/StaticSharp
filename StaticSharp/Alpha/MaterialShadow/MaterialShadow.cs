using StaticSharp.Gears;
namespace StaticSharp {
    public interface JMaterialShadow : JAbstractBoxShadow {
        public double Elevation  { get; set; }
    }

    [ConstructorJs]
    public partial class MaterialShadow : AbstractBoxShadow {

    }
}
