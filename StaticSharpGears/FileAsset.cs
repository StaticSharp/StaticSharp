using MimeTypes;
using StaticSharp.Gears;
using System.Text;

namespace StaticSharp {


    public interface IMutableAsset {
        Task<bool> GetValidAsync();
        public void DeleteCacheSubDirectory();
    }

    public record FileGenome(string Path) : AssetGenome<FileGenome, FileAsset> {
    }

    //Instead of inheritance from CacheableHttpRequest, it is better to use aggregation
    namespace Gears {

        public class FileAsset : Cacheable<FileGenome, FileAsset.Data>, IAsset, IMutableAsset {
            public class Data {
                public DateTime LastWriteTime;
                public string ContentHash = null!;
            };
            private DateTime GetLastWriteTime() {
                return File.GetLastWriteTimeUtc(Genome.Path);            
            }

            public Task<bool> GetValidAsync() => Task.FromResult(GetLastWriteTime() == CachedData.LastWriteTime);
            
            public string MediaType => MimeTypeMap.GetMimeType(FileExtension);
            public string ContentHash => CachedData.ContentHash;
            public string FileExtension => Path.GetExtension(Genome.Path);

            protected override Task CreateAsync() {
                if (!LoadData()) {
                    CachedData = new();

                    CachedData.LastWriteTime = GetLastWriteTime();
                    CachedData.ContentHash = Hash.CreateFromBytes(ReadAllBites()).ToString();

                    CreateCacheSubDirectory();
                    StoreData();
                }
                return Task.CompletedTask;
            }

            public override byte[] ReadAllBites() {
                if (Content == null) {
                    Content = FileUtils.ReadAllBytes(Genome.Path);
                }
                return Content;
            }

        }
    }
}

