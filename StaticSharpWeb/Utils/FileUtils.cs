
using System.IO;
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

        public static async Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default) {
            while (true) {
                try {
                    return await File.ReadAllTextAsync(path, cancellationToken);
                }
                catch (IOException) {
                    await Task.Yield();
                }
            }
        }


    }
}
