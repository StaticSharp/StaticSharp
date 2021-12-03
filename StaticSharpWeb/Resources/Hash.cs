using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpWeb.Resources {
    class Hash {
        public byte[] Data { get; private set; }

        Hash(byte[] data) => Data = data;

        public override string ToString() {
            const string Invalid = "INVALID_HASH";
            var data = Data;
            if(data != null && data.Length > 0) {
                var output = new StringBuilder();
                for(int i = 0; i < data.Length; i++) {
                    output.Append(data[i].ToString("X2"));
                }
                return output.ToString();
            }
            return Invalid;
        }

        private static HashAlgorithm HashAlgorithm => new MD5CryptoServiceProvider();

        private static async Task<Hash> CreateFromFileAsync(string path, HashAlgorithm algo) {
            using var fileStream = File.OpenRead(path);
            using var bufferedStream = new BufferedStream(fileStream, 1000000);
            return new Hash(await algo.ComputeHashAsync(bufferedStream));
        }

        public static async Task<Hash> CreateFromFileAsync(string path) {
            using var algo = HashAlgorithm;
            return await CreateFromFileAsync(path, algo);
        }

        public static async Task<Hash> CreateFromFilesAsync(IEnumerable<string> paths) {
            using var algo = HashAlgorithm;
            var hashes = new List<byte>(10 * 32);

            foreach(var path in paths) {
                hashes.AddRange((await CreateFromFileAsync(path, algo)).Data);
            }

            return new Hash(algo.ComputeHash(hashes.ToArray()));
        }

        public static async Task<Hash> CreateFromFilesAsync(string path, string searchPattern = "*") => 
            await CreateFromFilesAsync(Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories));

        public static Hash CreateFromString(string text) {
            using var algo = HashAlgorithm;
            var bytes = Encoding.Unicode.GetBytes(text);
            return new Hash(algo.ComputeHash(bytes));
        }
    }
}
