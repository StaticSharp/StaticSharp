using ImageMagick;
using StaticSharp.Gears;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {
    public record JpegGenome(Genome<Asset> Source, int? Quality = null) : ImageProcessorGenome(Source) {
        protected override Task<MagickImage> Process(MagickImage image) {
            if (Quality != null)
                image.Quality = Quality.Value;

            //https://stackoverflow.com/questions/19788127/save-a-jpeg-in-progressive-format

            image.Format = MagickFormat.Pjpeg;
            image.Strip();
            return Task.FromResult(image);
        }
    }

    /*namespace Gears {
        public class JpegAsset : ImageProcessorAsset<JpegGenome>, IImageAsset {
            public override string MediaType => "image/jpeg";
            public override string FileExtension => ".jpg";
            protected override Task<MagickImage> Process(MagickImage image) {
                if (Genome.Quality != null)
                    image.Quality = Genome.Quality.Value;

                //https://stackoverflow.com/questions/19788127/save-a-jpeg-in-progressive-format

                image.Format = MagickFormat.Pjpeg;
                image.Strip();
                return Task.FromResult(image);
            }
        }
    }*/
}

