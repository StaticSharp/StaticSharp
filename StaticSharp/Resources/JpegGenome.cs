using ImageMagick;
using StaticSharp.Gears;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {
    public record JpegGenome(Genome<IAsset> Source, int? Quality = null) : ImageProcessorGenome(Source) {
        protected override MagickImage Process(MagickImage image) {
            if (Quality != null)
                image.Quality = Quality.Value;

            //https://stackoverflow.com/questions/19788127/save-a-jpeg-in-progressive-format

            image.Format = MagickFormat.Pjpeg;
            image.Strip();
            return image;
        }
    }

}

