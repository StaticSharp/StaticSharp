using StaticSharp.Gears;
using System;
using System.IO;
using System.Threading.Tasks;






namespace StaticSharp {
    public record FileGenome(string Path) : Genome<IAsset> {
        class Data {
            public DateTime LastWriteTime;
            public string ContentHash = null!;
        };

        
        public override IAsset Create() {
            var result = new FileAsset(Path);

            if (!LoadData<Data>(out var data)) {
                result.LastWriteTime = data.LastWriteTime = FileAsset.GetLastWriteTime(Path);
                data.ContentHash = result.GetContentHash();
                CreateCacheSubDirectory();
                StoreData(data);
            } else {
                var lastWriteTime = FileAsset.GetLastWriteTime(Path);
                result.LastWriteTime = lastWriteTime;
                if (lastWriteTime == data.LastWriteTime) {
                    result.SetContentHash(data.ContentHash);
                }
            }
            return result;
        }

        


    }

    /*namespace Gears {

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
    }*/
}

