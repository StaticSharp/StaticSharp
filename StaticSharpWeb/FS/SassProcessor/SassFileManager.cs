using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibSassHost;
using System.Text;
using System.Security.Cryptography;

namespace StaticSharpWeb {

    public class SassFileManager : IFileManager {
        public bool SupportsConversionToAbsolutePath => throw new NotImplementedException();
        public bool FileExists(string path) => File.Exists(path);
        public string GetCurrentDirectory() => throw new NotImplementedException();
        public bool IsAbsolutePath(string path) => FileManager.Instance.IsAbsolutePath(path);
        public string ToAbsolutePath(string path) => throw new NotImplementedException();

        public string ReadFile(string path) {
            string content;
            while(true) {
                try {
                    content = File.ReadAllText(path);
                    break;
                } catch(IOException) {
                    
                }
            }
            return content;
        }
    }
}