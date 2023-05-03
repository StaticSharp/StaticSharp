using ImageMagick;

namespace StaticSharp.Gears;

public record PngGenome(Genome<IAsset> Source) : ImageProcessorGenome(Source) {
    protected override MagickImage Process(MagickImage image) {
        image.Format = MagickFormat.Png;
        image.Strip();
        return image;
    }
}
