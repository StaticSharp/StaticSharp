using MimeTypes;
using System;
using System.IO;
using System.Text;






namespace StaticSharp {

    namespace Gears {


        public class FileAsset : AssetSync, IMutableAsset {
            public string Path { get; }
            public DateTime LastWriteTime { get; set; }

            byte[]? data = null;

            string? contentHash = null;
            public FileAsset(string path) {
                Path = path;
            }

            public static DateTime GetLastWriteTime(string path) {
                return File.GetLastWriteTimeUtc(path);
            }

            public override string GetFileExtension() => System.IO.Path.GetExtension(Path);
            public override string GetMediaType() {
                return MimeTypeMap.GetMimeType(GetFileExtension());
            }
            public void SetContentHash(string value) {
                contentHash = value;
            }
            public override string GetContentHash() {
                if (contentHash == null) {
                    contentHash = Hash.CreateFromBytes(GetBytes()).ToString();
                }
                return contentHash;
            }
            public override byte[] GetBytes() {
                if (data == null) {
                    using (var fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read)) {
                        data = new byte[fileStream.Length];
                        fileStream.Read(data, 0, data.Length);
                    }
                }
                return data;
            }
            public override string GetText() {
                using (MemoryStream memoryStream = new(GetBytes())) {
                    using (StreamReader streamReader = new StreamReader(memoryStream, true)) {
                        var text = streamReader.ReadToEnd();
                        return text;
                    }
                }
            }

            public bool GetValid() {
                return LastWriteTime == GetLastWriteTime(Path);
            }

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

