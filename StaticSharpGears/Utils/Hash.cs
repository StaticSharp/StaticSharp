using System.Security.Cryptography;
using System.Text;

namespace StaticSharp.Gears;

public class Hash {

    private static HashAlgorithm HashAlgorithm = MD5.Create();

    public byte[] Data { get; }

    Hash(byte[] data) => Data = data;

    private static async Task<Hash> CreateFromFileAsync(string path) {
        using var fileStream = File.OpenRead(path);
        using var bufferedStream = new BufferedStream(fileStream, 1024*1024);
        return new Hash(await HashAlgorithm.ComputeHashAsync(bufferedStream));
    }

    public static async Task<Hash> CreateFromFilesAsync(IEnumerable<string> paths) {
        var hashes = new List<byte>(10 * 32);
        foreach(var path in paths) {
            hashes.AddRange((await CreateFromFileAsync(path)).Data);
        }
        return new Hash(HashAlgorithm.ComputeHash(hashes.ToArray()));
    }

    public static async Task<Hash> CreateFromFilesAsync(string path, string searchPattern = "*") => 
        await CreateFromFilesAsync(Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories));

    public static Hash CreateFromString(string text) {
        var bytes = Encoding.Unicode.GetBytes(text);
        return new Hash(HashAlgorithm.ComputeHash(bytes));
    }
    public static Hash CreateFromBytes(byte[] bytes) {
        return new Hash(HashAlgorithm.ComputeHash(bytes));
    }
    public static Hash CreateFromStream(Stream stream) {
        //var position = stream.Position;
        stream.Seek(0, SeekOrigin.Begin);
        var result = new Hash(HashAlgorithm.ComputeHash(stream));
        //stream.Position = position;
        return result;
    }

    static string ByteToHexBitFiddle(byte[] bytes) { //TODO: place somewhere else (maybe)
        char[] chars = new char[bytes.Length * 2];
        int b;
        for (int i = 0; i < bytes.Length; i++) {
            b = bytes[i] >> 4;
            chars[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
            b = bytes[i] & 0xF;
            chars[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
        }
        return new string(chars);
    }

    public override string ToString() {
        //if (Data == null) return null;
        if (Data.Length == 0) return "";
        return ByteToHexBitFiddle(Data);

    }

}

