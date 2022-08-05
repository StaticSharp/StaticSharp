using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibSassHost;
using System.Text;
using System.Security.Cryptography;
using StaticSharp.Utils;

namespace StaticSharp.Gears {
    public class SassProcessor : IFileManager {
        public string DirectoryPath = "style.scss";
        public SassProcessor(string directoryPath) {
            DirectoryPath = directoryPath;
        }
        public SassProcessor() {

        }
        private static IFileManager GetFileManager(string extension) => extension switch {
            _ => new SassFileManager(),
        };
        public bool SupportsConversionToAbsolutePath => false;
        public bool FileExists(string path) => GetFileManager(Path.GetExtension(path)).FileExists(path);
        public string GetCurrentDirectory() => DirectoryPath;
        public bool IsAbsolutePath(string path) => FileManager.Instance.IsAbsolutePath(path);
        public string ToAbsolutePath(string path) => null;        
        public string ReadFile(string path) {
            StringBuilder stringBuilder = new();

            stringBuilder.AppendLine(GetFileManager(Path.GetExtension(path)).ReadFile(path));
            stringBuilder.AppendLine($"$ss-file-path-hash: \"{path.Replace("\\", "/").ToHashString()}\";");
            stringBuilder.AppendLine($"$ss-file-hash: \"{File.ReadAllText(path).ToHashString()}\";");
            stringBuilder.AppendLine($"$ss-file-path: \"{path}\";");
            stringBuilder.AppendLine($"$ss-file-name: \"{Path.GetFileName(path)}\";");
            stringBuilder.AppendLine($"$ss-directory: \"{Path.GetDirectoryName(DirectoryPath.Replace("\\", "/"))}\";");
            return stringBuilder.ToString();
        }
        public string Update(string styleList) {
            CompilationResult result = new();
            //try {
                var options = new CompilationOptions { SourceMap = true };
                options.OutputStyle = OutputStyle.Compact;
                options.SourceMapFileUrls = true;
                SassCompiler.FileManager = this;
                result = SassCompiler.Compile(styleList, options);
            /*} catch {
                
            }*/
            return result.CompiledContent;
        }
    }
}