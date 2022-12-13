using ImageMagick;
using StaticSharp.Gears;

namespace StaticSharp;

public partial class Static {
    public static Genome<IAsset> GetCodeRegion(this Genome<IAsset> genome, string RegionName, bool trim = true) {
        return new CodeRegionGenome(genome, RegionName, trim);
    }
}
