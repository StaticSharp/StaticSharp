using ImageMagick;
using StaticSharp.Gears;
using System.IO;
using System.Threading.Tasks;

namespace StaticSharp {


    public interface IImageProcessorGenome {
        IGenome<IAsset> Source { get; }
    }
    public abstract record ImageProcessorGenome<TFinalGenome, TCacheable>(IGenome<IAsset> Source) : ImageGenome<TFinalGenome, TCacheable>, IImageProcessorGenome
    where TFinalGenome : ImageProcessorGenome<TFinalGenome, TCacheable>
    where TCacheable : ICacheable<TFinalGenome>, IImageAsset, new() {

    }

    namespace Gears {

        public class ImageProcessorAssetData {
            public string ContentHash = null!;
            public string SourceHash = null!;
            public int Width;
            public int Height;
        };

        public abstract class ImageProcessorAsset<TGenome, TData> : ImageAsset<TGenome, TData>, IMutableAsset
            where TGenome : class, IKeyProvider, IImageProcessorGenome
            where TData : ImageProcessorAssetData, new() {
            

            public abstract string MediaType { get; }

            public string ContentHash => CachedData.ContentHash;

            public abstract string FileExtension { get; }

            public int Width => CachedData.Width;
            public int Height => CachedData.Height;

            public async Task<bool> GetValidAsync() {
                var source = await Genome.Source.CreateOrGetCached();
                return source.ContentHash == CachedData.SourceHash;
            }

            bool VerifyCachedData() {
                return CachedData.ContentHash != null
                    && CachedData.SourceHash != null;
            }


            protected abstract Task<MagickImage> Process(MagickImage source);

            protected override async Task CreateAsync() {

                if (!LoadData() || !VerifyCachedData()) {
                    CachedData = new();

                    var source = await Genome.Source.CreateOrGetCached();
                    CachedData.SourceHash = source.ContentHash;

                    var image = new MagickImage(source.ReadAllBites());

                    image = await Process(image);                    

                    CachedData.Width = image.Width;
                    CachedData.Height = image.Height;                    

                    CreateCacheSubDirectory();

                    Content = image.ToByteArray();
                    File.WriteAllBytes(ContentFilePath, Content); //TODO: mb async?
                    CachedData.ContentHash = Hash.CreateFromBytes(Content).ToString();

                    StoreData();
                }
            }


        }
    }
}

