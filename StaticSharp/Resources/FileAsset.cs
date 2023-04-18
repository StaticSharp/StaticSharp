


namespace StaticSharp {

    namespace Gears {
        public class FileAsset : IAsset {

            public string Path { get; }
            //public DateTime LastWriteTime { get; set; }

            byte[]? data = null;

            string? contentHash;
            public FileAsset(string path, string? contentHash = null) {
                Path = path;
                this.contentHash = contentHash;
            }

            public static DateTime GetLastWriteTime(string path) {
                return File.GetLastWriteTimeUtc(path);
            }

            public string Extension => System.IO.Path.GetExtension(Path);

            public string ContentHash {
                get {
                    if (contentHash == null) {
                        contentHash = Hash.CreateFromBytes(Data).ToString();
                    }
                    return contentHash;
                }
            }

            public byte[] Data {
                get {
                    if (data == null) {
                        data = FileUtils.ReadAllBytes(Path);
                    }
                    return data;
                }
            }
            public string Text {
                get {
                    using (MemoryStream memoryStream = new(Data)) {
                        using (StreamReader streamReader = new StreamReader(memoryStream, true)) {
                            var text = streamReader.ReadToEnd();
                            return text;
                        }
                    }
                }
            }
        }
    }
}

