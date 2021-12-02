using CsmlWeb.Html;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CsmlWeb {

    public class RelativePath {
        string Value { get; init; }
        public RelativePath(string subPath = "", [CallerFilePath] string callerFilePath = "") => 
            Value = Path.Combine(Path.GetDirectoryName(callerFilePath), subPath);

        public static implicit operator string(RelativePath relativePath) => relativePath.Value;

        public override string ToString() => Value;

    }


    internal class JSCall {
        public string Path { get; init; }
        public readonly object[] _parameters;


        public JSCall(string path, params object[] parameters) 
            => (Path, _parameters) = (path, parameters);



        public Tag Generate(Context context) {
            var functionName = System.IO.Path.GetFileNameWithoutExtension(Path);

            context.Includes.Require(new Script(Path));

            var parameters = string.Concat(_parameters.Select(x => "," + JsonConvert.SerializeObject(x)));

            var code = $@"CsmlCall(function(parent){{{functionName}(parent {parameters})}})";

            return new("script") {
                new PureHtmlNode(code)
            };
        }
    }
}