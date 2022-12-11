using StaticSharp.Gears;
using System.IO;
using System.Runtime.CompilerServices;
using System;

namespace StaticSharp;
    public partial class Static {
        public static Genome<IAsset> LoadFile(string pathOrUrl, [CallerFilePath] string callerFilePath = "") {
            if (File.Exists(pathOrUrl)) {
                return new FileGenome(pathOrUrl);
            }
            var absolutePath = MakeAbsolutePath(pathOrUrl, callerFilePath);
            if (File.Exists(absolutePath)) {
                return new FileGenome(absolutePath);
            }
            if (Uri.TryCreate(pathOrUrl, UriKind.Absolute, out var uri)) {
                return new HttpRequestGenome(uri);
            }
            //TODO: 
            throw new FileNotFoundException("File or Url not found", pathOrUrl);
        }
    }
