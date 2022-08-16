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


            /*var relativePath = Path.GetRelativePath(ProjectDirectory.Path, callerFilePath);
            var resourceDirectory = Path.GetDirectoryName(relativePath)?.Replace(Path.PathSeparator, '.');
            if (string.IsNullOrEmpty(resourceDirectory)) {
                ResourceDirectory = "";
            } else {
                ResourceDirectory = resourceDirectory + ".";
            }*/
        }
        public abstract string Extension { get; }
        /*public string? Get(Type type) {
            var assembly = type.Assembly;
            var resourcePath = assembly.GetName().Name + "." + ResourceDirectory + type.Name + Extension;
            using var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null) {
                return null;
            }
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }*/
    }
}
