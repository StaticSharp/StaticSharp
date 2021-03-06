using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibSassHost;
using System.Text;
using System.Security.Cryptography;

namespace StaticSharp.Gears {
    public interface IStyle : IInclude {
        public string Path { get; }
    }

    public class Style : IStyle {

        public string Path { get; }

        public string Key => GetType().FullName + '\0' + Path;


        private IEnumerable<IStyle> _dependencies = Enumerable.Empty<IStyle>();

        public IEnumerable<IStyle> Dependencies => _dependencies;


        public Style(string path) => Path = path;

        public virtual async Task<string> GenerateIncludeAsync() {
            try {
                var compilerResult = SassCompiler.Compile(await File.ReadAllTextAsync(Path));
                _dependencies = compilerResult.IncludedFilePaths.Select(x => new Style(x));
                return compilerResult.CompiledContent;
            } catch(SassCompilationException ex) {
                Console.WriteLine(ex);
                throw;
            } catch(Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }

        public string GenerateSuperStyle(string styleList) {
            try {
                SassProcessor SassProcessor = new();
                return SassProcessor.Update(styleList);
            } catch (Exception ex){
                Console.WriteLine(ex);
                throw;
            }
        }
    }


}