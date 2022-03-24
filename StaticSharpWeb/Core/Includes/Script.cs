using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface IScript : IInclude {
        IEnumerable<IScript> Dependencies { get; }
        public string Path { get; }
    }

    public class Script : IScript {
        public string Path { get; }

        public string Key => GetType().FullName + '\0' + Path;


        public IEnumerable<IScript> Dependencies => Enumerable.Empty<IScript>();


        public Script(string path) => Path = path;

        public virtual async Task<string> GenerateIncludeAsync(IStorage storage) => await File.ReadAllTextAsync(Path);

        public string GenerateSuperScript(string[] scripts) {
            StringBuilder stringBuilder = new();
            try {
                foreach(var i in scripts) {
                    var script = ReadFile(i);
                    stringBuilder.AppendLine(script);
                }
                return stringBuilder.ToString();
            } catch(Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }

        public string ReadFile(string script) {

            var file = File.ReadAllText(script);
            var thisFilePath = AbsolutePath(script);

            string result = 
                            file.Replace("☺thisFilePathHash☹", thisFilePath.ToString().ToHashString())
                            .Replace("☺thisFileHash☹", file.ToHashString())
                            .Replace("☺thisFilePath☹", thisFilePath.ToString())
                            .Replace("☺thisFileName☹", System.IO.Path.GetFileName(thisFilePath))
                            .Replace("☺thisDirectory☹", System.IO.Path.GetDirectoryName(thisFilePath));


            /*var uglifyResult = NUglify.Uglify.Js(result, script);
            if (uglifyResult.HasErrors) {
                throw new Exception("Javascript Uglify error's: "
                        + "\n\t"
                        + string.Join("\n\t", uglifyResult.Errors));
            }*/
            return result;
        }
    }
}