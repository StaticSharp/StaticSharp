using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace StaticSharp {

    namespace Gears {
        public class BinaryAsset : IAsset {
            string extension;
            string? contentHash;
            byte[] data;

            public BinaryAsset(byte[] data, string extension, string? contentHash = null ) {
                this.extension = extension;
                this.data = data;
                this.contentHash = contentHash;
            }

            public string Extension => extension;
            public string ContentHash {
                get {
                    if (contentHash == null) {
                        contentHash = Hash.CreateFromBytes(data).ToString();
                    }
                    return contentHash;
                }
            }
            public byte[] Data => data;
            public string Text => Encoding.UTF8.GetString(data);

        }
    }


}

