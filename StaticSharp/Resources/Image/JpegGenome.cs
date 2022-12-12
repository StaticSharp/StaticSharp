using ImageMagick;

namespace StaticSharp.Gears;

public record JpegGenome(Genome<IAsset> Source, int? Quality = null) : ImageProcessorGenome(Source) {
    protected override MagickImage Process(MagickImage image)
    {
        if (Quality != null)
            image.Quality = Quality.Value;

        //https://stackoverflow.com/questions/19788127/save-a-jpeg-in-progressive-format

        image.Format = MagickFormat.Pjpeg;
        image.Strip();
        return image;
    }
}


