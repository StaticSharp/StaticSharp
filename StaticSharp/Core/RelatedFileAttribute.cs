using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp.Gears {

    public abstract class RelatedFileAttribute : Attribute {
        public string FilePathOrExtension { get; }
        public string CallerFilePath { get; }

        public RelatedFileAttribute(string filePathOrExtension, string callerFilePath) {
            CallerFilePath = callerFilePath;
            FilePathOrExtension = filePathOrExtension;
        }
        //public abstract string Extension { get; }

        public static Genome<IAsset> GetGenome(Type type, string filePathOrExtension, [CallerFilePath] string callerFilePath = "") {
            var assembly = type.Assembly;
            var typeName = type.Name;

            var extension = Path.GetExtension(filePathOrExtension);
            var filePath =
                extension == filePathOrExtension
                ? typeName + extension
                : filePathOrExtension;


            string absoluteFilePath;
            if (Path.IsPathRooted(filePath)) {
                absoluteFilePath = filePath;
            } else {
                string directory = Path.GetDirectoryName(callerFilePath) ?? "";
                absoluteFilePath = Path.GetFullPath(Path.Combine(directory, filePath));
            }


            if (File.Exists(absoluteFilePath)) {
                var result = new FileGenome(absoluteFilePath);
                return result;

            } else {
                var relativeFilePath = AssemblyResourcesUtils.GetFilePathRelativeToProject(assembly, absoluteFilePath);
                var relativeResourcePath = AssemblyResourcesUtils.GetResourcePath(relativeFilePath);

                var result = new AssemblyResourceGenome(assembly, relativeResourcePath);
                return result;

            }
        }

        public virtual Genome<IAsset> GetGenome(Type type) {
            return GetGenome(type, FilePathOrExtension, CallerFilePath);
        }

    }
}
