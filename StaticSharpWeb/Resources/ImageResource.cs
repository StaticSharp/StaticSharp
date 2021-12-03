using StaticSharpWeb.Html;
using ImageMagick;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharpWeb.Resources {



    public class ImageResource : IResource {


        private string _inputFilePath;
        private Hash _hash;
        private string _storageDirectory;

        public float[] Roi;
        public bool IsVectorImage;
        public bool IsAnimatedImage;
        public Dictionary<int, string> Mips;
        public IMagickColor<ushort> TopLeftPixelColor;
        public const int MinImageWidth = 32;
        private string OutputDirectory => Path.Combine(_storageDirectory, _hash?.ToString() ?? "empty");

        public float Aspect { get; private set; }
        public string Key => _inputFilePath;
        public string Source => Path.Combine(OutputDirectory, Mips.Values.Last()).Replace(_storageDirectory, "");

        public ImageResource(string filePath, IStorage storage) {
            _inputFilePath = filePath;
            _storageDirectory = storage.StorageDirectory;
            //OutputDirectory = Path.Combine(storage.StorageDirectory, Path.GetFileNameWithoutExtension(_inputFilePath));
        }

        private async Task<float[]> ReadRoi() {
            var roiPath = Path.Combine(Path.GetDirectoryName(_inputFilePath), Path.GetFileNameWithoutExtension(_inputFilePath) + ".roi");
            if (!File.Exists(roiPath)) {
                return System.Array.Empty<float>();
            }
            var roiFile = await File.ReadAllTextAsync(roiPath);
            var roi = Newtonsoft.Json.JsonConvert.DeserializeObject<float[]>(roiFile);
            return roi;
        }

        private async Task<bool> Load() {
            if (!Directory.Exists(OutputDirectory)) {
                return false;
            }

            Roi ??= await ReadRoi();
            Mips = new();
            foreach (var file in Directory.EnumerateFiles(OutputDirectory)) {
                var splittedName = Path.GetFileName(file).Split("_", System.StringSplitOptions.RemoveEmptyEntries);
                if (!int.TryParse(splittedName[1], out var size)) {
                    return false;
                }
                Mips.Add(size, splittedName[0]);
            }
            return true;
        }

        public async Task GenerateAsync() {
            if (!File.Exists(_inputFilePath)) {
                throw new FileNotFoundException($"Could not find file: {_inputFilePath}");
            }
            _hash ??= await Hash.CreateFromFileAsync(_inputFilePath);
            if (Mips != null || await Load()) {
                return;
            }
            var extension = Path.GetExtension(_inputFilePath);
            var direcotry = _storageDirectory;
            var hash = _hash.ToString();
            var roiTask = ReadRoi();
            string outputFileName(int mip) => $"{hash}_{mip}{extension}";
            string outputPathFunc(int mip) => Path.Combine(direcotry, hash, outputFileName(mip));
            using var image = new MagickImage(_inputFilePath);

            TopLeftPixelColor = image.GetPixels()[0, 0].ToColor();
            Aspect = image.Height / (float)image.Width;
            Mips = new();
            IsVectorImage = image.Format == MagickFormat.Svg;
            IsAnimatedImage = image.Format == MagickFormat.Gif;

            var mipWidth = image.Width;
            var outputPath = outputPathFunc(mipWidth);
            if (File.Exists(outputPath)) { File.Delete(outputPath); }
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            File.Copy(_inputFilePath, outputPath);
            Mips.Add(mipWidth, outputFileName(mipWidth));

            if (!(IsVectorImage || IsAnimatedImage)) {
                while (MinImageWidth <= mipWidth / 2) {
                    image.Resize(image.Width / 2, image.Height / 2);
                    await image.WriteAsync(outputPathFunc(image.Width));
                    //PngUtils.NormalizeChunks(outputPath(image.Width)); //to prevent commit after every conversion;
                    mipWidth = image.Width;
                    Mips.Add(mipWidth, outputFileName(mipWidth));
                }
            }
            Roi = await roiTask;
        }
    }
}