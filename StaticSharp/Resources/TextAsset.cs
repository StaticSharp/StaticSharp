using System.Text;

namespace StaticSharp {

    namespace Gears {
        public class TextAsset : IAsset {
            string extension;
            string? contentHash;
            string text;

            public TextAsset(string text, string extension, string? contentHash = null) {
                this.extension = extension;
                this.text = text;
                this.contentHash = contentHash;
            }

            public string Extension => extension;
            public string ContentHash {
                get {
                    if (contentHash == null) {
                        contentHash = Hash.CreateFromString(text).ToString();
                    }
                    return contentHash;
                }
            }
            public byte[] Data => Encoding.UTF8.GetBytes(text);
            public string Text => text;
        }
    }
}

