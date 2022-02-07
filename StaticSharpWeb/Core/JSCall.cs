using StaticSharpWeb.Html;
using Newtonsoft.Json;
using System.Linq;

namespace StaticSharpWeb {

    internal class JSCall {
        public string Path { get; init; }
        public readonly object[] _parameters;


        public JSCall(string path, params object[] parameters) 
            => (Path, _parameters) = (path, parameters);


        public Tag Generate(Context context) {
            var functionName = System.IO.Path.GetFileNameWithoutExtension(Path);

            context.Includes.Require(new Script(Path));

            var parameters = string.Concat(_parameters.Select(x => "," + JsonConvert.SerializeObject(x)));

            var code = $@"StaticSharpCall(function(parent){{{functionName}(parent {parameters})}})";

            return new("script") {
                new PureHtmlNode(code)
            };
        }
    }
}