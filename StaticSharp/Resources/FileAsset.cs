using MimeTypes;
using StaticSharp.Gears;
using System;
using System.IO;
using System.Threading.Tasks;






namespace StaticSharp {

    public record FileGenome(string Path) : Genome<Asset> {

        class Data {
            public DateTime LastWriteTime;
            public string ContentHash = null!;
        };
        public override Task<Asset> CreateAsync() {
            if (!LoadData<Data>(out var data)) {

                data.LastWriteTime = GetLastWriteTime();
                data.ContentHash = Hash.CreateFromBytes(ReadAllBites()).ToString();

                CreateCacheSubDirectory();
                StoreData(data);
            }
            ContentHash = data.ContentHash;
            LastWriteTime = data.LastWriteTime;

            return Task.CompletedTask;
        }
    }

    namespace Gears {

        public class FileAsset : CacheableToFile<FileGenome>, Asset, IMutableAsset {
            class Data {
                public DateTime LastWriteTime;
                public string ContentHash = null!;
            };
            private DateTime GetLastWriteTime() {
                return File.GetLastWriteTimeUtc(Genome.Path);            
            }

            public Task<bool> GetValidAsync() => Task.FromResult(GetLastWriteTime() == LastWriteTime);
            
            public string MediaType => MimeTypeMap.GetMimeType(FileExtension);
            public string ContentHash { get; private set; } = null!;
            public string FileExtension => Path.GetExtension(Genome.Path);
            public DateTime LastWriteTime { get; private set; }
            protected override Task CreateAsync() {
                if (!LoadData<Data>(out var data)) {
                    
                    data.LastWriteTime = GetLastWriteTime();
                    data.ContentHash = Hash.CreateFromBytes(ReadAllBites()).ToString();

                    CreateCacheSubDirectory();
                    StoreData(data);
                }
                ContentHash = data.ContentHash;
                LastWriteTime = data.LastWriteTime;

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

