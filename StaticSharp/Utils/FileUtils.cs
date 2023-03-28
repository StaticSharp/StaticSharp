
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StaticSharp.Gears {

    public static class FileUtils {

        public static string ReadAllText(string path) {
            while (true) {
                try {
                    return File.ReadAllText(path);
                }
                catch (IOException) {
                    Thread.Yield();
                }
            }
        }

        public static string ReadAllText(byte[] data, string? CharSet) {
            using (MemoryStream memoryStream = new(data)) {
                if (CharSet != null) {
                    Encoding encoding = Encoding.GetEncoding(CharSet);
                    using (StreamReader streamReader = new StreamReader(memoryStream, encoding, true)) {
                        var text = streamReader.ReadToEnd();
                        return text;
                    }
                } else {
                    using (StreamReader streamReader = new StreamReader(memoryStream, true)) {
                        var text = streamReader.ReadToEnd();
                        return text;
                    }
                }
            }
        }


        public static byte[] ReadAllBytes(string path) {
            while (true) {
                try {
                    return File.ReadAllBytes(path);
                }
                catch (IOException) {
                    Thread.Yield();
                }
            }
        }

        public static void WriteAllBytes(string path, byte[] bytes) {
            while (true) {
                try {
                    File.WriteAllBytes(path, bytes);
                    return;
                }
                catch (IOException) {
                    Thread.Yield();
                }
            }
        }

    }
}
