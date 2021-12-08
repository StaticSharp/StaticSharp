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

        public virtual async Task<string> GenerateAsync(IStorage storage) => await File.ReadAllTextAsync(Path);

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
            var thisFilePath = new RelativePath(script);
            string result = "// START FILE " + script + "\n" +
                            File.ReadAllText(script).Replace("☺thisFilePath☹", thisFilePath)
                            + "\n// END FILE " + script + "\n";
            var uglifyResult = NUglify.Uglify.Js(result, script);
            if (uglifyResult.HasErrors) {
                throw new Exception("Javascript Uglify error's: "
                        + "\n\t"
                        + string.Join("\n\t", uglifyResult.Errors));
            }
            return result;
        }
    }
}