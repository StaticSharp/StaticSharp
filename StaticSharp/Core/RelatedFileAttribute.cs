using System;
using System.IO;
using System.Runtime.CompilerServices;


namespace StaticSharp.Gears {

    public abstract class RelatedFileAttribute : Attribute {
        public string? FileName { get; }
        public string CallerFilePath { get; }

        public RelatedFileAttribute(string? fileName, string callerFilePath) {
            CallerFilePath = callerFilePath;
            FileName = fileName;
        }
        public abstract string Extension { get; }

    }
}
