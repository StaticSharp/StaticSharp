using StaticSharp.Gears;
using StaticSharp.Html;
using StaticSharpWeb.Html;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;


namespace StaticSharpWeb.Html {
    

}



namespace StaticSharpWeb {


    public class JSCall {

        private static readonly JsonSerializerOptions JsonSerializerOptions = new() {
            IncludeFields = true,
        };
        public string FunctionName { get; }
        public object? Parameters { get; }

        public JSCall(string functionName, object? parameters = null) {
            FunctionName = functionName;
            Parameters = parameters;
        }

        public Tag Generate(Context context) {


            string parametersJson = "{}";
            if (Parameters != null) {
                parametersJson = JsonSerializer.Serialize(Parameters, JsonSerializerOptions);
            }

            return new Tag("script"){
                new PureHtmlNode($"StaticSharpCall({FunctionName},{parametersJson});")
            };
        }
    }

    /*internal class JSCall {

        private static readonly JsonSerializerOptions JsonSerializerOptions = new() {
            IncludeFields = true,
        };

        public string Path { get; init; }
        public readonly object? Parameters;


        public JSCall(string path, object? parameters = null) 
            => (Path, Parameters) = (path, parameters);


        public Tag Generate(Context context) {
            var functionName = System.IO.Path.GetFileNameWithoutExtension(Path);

            context.Includes.Require(new Script(Path));

            string parametersJson = "{}";
            if (Parameters != null) {
                parametersJson = JsonSerializer.Serialize(Parameters, JsonSerializerOptions);
            }


            //var parameters = string.Concat(Parameters.Select(x => "," + JsonConvert.SerializeObject(x)));

            //var code = $@"StaticSharpCall(function(parent){{{functionName}(parent, {parametersJson})}})";

            
            var code = $"StaticSharpCall({functionName},{parametersJson});";//


            return new("script") {
                new PureHtmlNode(code)
            };
        }
    }*/
}