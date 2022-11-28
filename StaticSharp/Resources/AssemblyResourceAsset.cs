using MimeTypes;
using StaticSharp.Gears;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;



namespace StaticSharp {
    public record AssemblyResourceGenome(Assembly Assembly, string Path) : Genome<IAsset> {
        public override Task<IAsset> CreateAsync() {
            /*if (!LoadData<Data>(out var data)) {
                data.ContentHash = Hash.CreateFromBytes(ReadAllBites()).ToString();
                CreateCacheSubDirectory();
                StoreData(data);
            }
            ContentHash = data.ContentHash;*/

            var resourcePath = Assembly.GetName().Name + "." + Path;
            using var stream = Assembly.GetManifestResourceStream(resourcePath);
            //throw

            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                var result = new AssemblyResourceAsset(memoryStream.ToArray(), Path);

                return Task.FromResult<IAsset>(result);
            }



           /* var extension = System.IO.Path.GetExtension(Path);


            return Task.FromResult(new Asset(
                () => {
                    var resourcePath = Assembly.GetName().Name + "." + Path;
                    using var stream = Assembly.GetManifestResourceStream(resourcePath);
                    //throw

                    using (var memoryStream = new MemoryStream()) {
                        stream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                },
                extension,
                MimeTypeMap.GetMimeType(extension)
                ));*/
        }

        public class AssemblyResourceAsset : AssetSync {
            public string Path { get; }

            byte[] data;

            string? contentHash = null;
            public AssemblyResourceAsset(byte[] data, string path) {
                this.data = data;
                Path = path;
            }
            public override string GetFileExtension() => System.IO.Path.GetExtension(Path);

            public override string GetMediaType() {
                return MimeTypeMap.GetMimeType(GetFileExtension());
            }
            public override string GetContentHash() {
                if (contentHash == null) {
                    contentHash = Hash.CreateFromBytes(GetBytes()).ToString();
                }
                return contentHash;
            }
            public override byte[] GetBytes() => data;            
            public override string GetText() {
                using (MemoryStream memoryStream = new(GetBytes())) {
                    using (StreamReader streamReader = new StreamReader(memoryStream, true)) {
                        var text = streamReader.ReadToEnd();
                        return text;
                    }
                }
            }
        }


    }

    namespace Gears {

        public static partial class KeyCalculators {
            public static string GetKey(Assembly assembly) {
                return KeyUtils.Combine<Assembly>(assembly.FullName);
            }
        }


        /*public class AssemblyResourceAsset : CacheableToFile<AssemblyResourceGenome>, Asset {
            class Data {
                public string ContentHash = null!;
            };
            public string MediaType => MimeTypeMap.GetMimeType(FileExtension);
            public string ContentHash { get; private set; } = null!;
            public string FileExtension => Path.GetExtension(Genome.Path);

            protected override Task CreateAsync() {
                if (!LoadData<Data>(out var data)) {
                    data.ContentHash = Hash.CreateFromBytes(ReadAllBites()).ToString();
                    CreateCacheSubDirectory();
                    StoreData(data);
                }
                ContentHash = data.ContentHash;
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

        }*/
    }
}

