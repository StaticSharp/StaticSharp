using MimeTypes;
using StaticSharp.Gears;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;



namespace StaticSharp {
    public record AssemblyResourceGenome(Assembly Assembly, string Path) : AssetGenome<AssemblyResourceGenome, AssemblyResourceAsset> {

    }

    //Instead of inheritance from CacheableHttpRequest, it is better to use aggregation
    namespace Gears {

        public static partial class KeyCalculators {
            public static string GetKey(Assembly assembly) {
                return KeyUtils.Combine<Assembly>(assembly.FullName);
            }
        }


        public class AssemblyResourceAsset : Cacheable<AssemblyResourceGenome, AssemblyResourceAsset.Data>, IAsset {
            public class Data {
                public string ContentHash = null!;
            };
            public string MediaType => MimeTypeMap.GetMimeType(FileExtension);
            public string ContentHash => CachedData.ContentHash;
            public string FileExtension => Path.GetExtension(Genome.Path);

            protected override Task CreateAsync() {
                if (!LoadData()) {
                    CachedData = new();
                    CachedData.ContentHash = Hash.CreateFromBytes(ReadAllBites()).ToString();
                    CreateCacheSubDirectory();
                    StoreData();
                }
                return Task.CompletedTask;
            }

            public override byte[] ReadAllBites() {
                if (Content == null) {
                    var resourcePath = Genome.Assembly.GetName().Name + "." + Genome.Path;
                    using var stream = Genome.Assembly.GetManifestResourceStream(resourcePath);
                    //throw

                    using (var memoryStream = new MemoryStream()) {
                        stream.CopyTo(memoryStream);
                        Content = memoryStream.ToArray();
                    }
                }
                return Content;
            }

        }
    }
}

