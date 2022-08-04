using StaticSharp.Gears;
using System.IO;
using System.Threading.Tasks;

namespace StaticSharp {

    public interface IImageAsset : IAsset {
        int Width { get; }
        int Height { get; }
    }


    public abstract record ImageGenome<TFinalGenome, TCacheable> : AssetGenome<TFinalGenome, TCacheable>, IGenome<IImageAsset>
    where TFinalGenome : ImageGenome<TFinalGenome, TCacheable>
    where TCacheable : ICacheable<TFinalGenome>, IImageAsset, new() {

        async Task<IImageAsset> IGenome<IImageAsset>.CreateAsync() {
            return await base.CreateAsync();
        }
        async Task<IImageAsset> IGenome<IImageAsset>.CreateOrGetCached() {
            return await base.CreateOrGetCached();
        }
    }

    namespace Gears {
        public abstract class ImageAsset<TGenome, TData> : Cacheable<TGenome, TData>
            where TGenome : class, IKeyProvider
            where TData : class, new() {

            public override string? CharSet => null;

        }
    }
}

