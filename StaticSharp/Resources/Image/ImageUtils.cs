using ImageMagick;

namespace StaticSharp.Gears;

public static class ImageUtils {

    public static Genome<IAsset> ToWebImage(this Genome<IAsset> genome) {
        string[] webExtensions = { ".jpg", ".jpeg", ".png", ".svg" };
        var asset = genome.Result;
        if (!webExtensions.Contains(asset.Extension)) {
            return new JpegGenome(genome);
        }
        return genome;
    }


    public static MagickImageInfo GetImageInfo(this IAsset source) {
        return new MagickImageInfo(source.Data);
    }


}