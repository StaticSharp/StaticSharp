using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StaticSharp.Gears {
    public static class AssemblyResourcesUtils {
        public static string GetProjectPath(Assembly assembly) {
            //TODO: exceptions

            var type = assembly.GetType("ProjectDirectory");
            var property = type.GetProperty("Path");
            var result = property.GetValue(null);
            return result as string;
        }

        public static string GetFilePathRelativeToProject(Assembly assembly, string absolutePath) {
            return Path.GetRelativePath(GetProjectPath(assembly), absolutePath);
        }

        public static string GetResourcePath(string relativeFilePath) {
            return relativeFilePath.Replace(Path.DirectorySeparatorChar, '.');
        }

        /*public static string GetResourcePathInThisDirectory(string fileName, Assembly assembly, [CallerFilePath] string callerFilePath = "") {            
            string directory = Path.GetDirectoryName(callerFilePath);
            string absoluteFilePath = Path.Combine(directory, fileName);
            var relativeFilePath = GetFilePathRelativeToProject(assembly, absoluteFilePath);
            var relativeResourcePath = GetResourcePath(relativeFilePath);
            return relativeResourcePath;
        }*/


    }
}
