using Microsoft.Extensions.FileProviders;
using StaticSharp.Core;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StaticSharp {


    namespace Gears {


        [System.Diagnostics.DebuggerNonUserCode]
        public class ReactiveJs {
        }


        



        public class Bindings<FinalJs> where FinalJs : new() {
            public class Binding<T> : IVoidEnumerable {

                private T? Value;
                private Expression<Func<FinalJs, T>>? Expression;

                public Binding(Expression<Func<FinalJs, T>> expression) {
                    Expression = expression;
                }
                public Binding(T value) {
                    Value = value;
                }

                public static implicit operator Binding<T>(T value) {
                    return new Binding<T>(value);
                }
                public string CreateScriptExpression() {
                    if (Expression != null) {
                        return new BindingScriptifier(Expression, new FinalJs()).Eval();
                    }
                    return Static.ObjectToJsValue(Value);
                }
            }

            protected void Apply<T>(Binding<T> binding, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") {
                var aggregator = (Aggregator.Current as Reactive);
                if (aggregator == null)
                    throw new InvalidOperationException($"{nameof(Bindings<FinalJs>)} must be aggregated into {nameof(Reactive)} only");

                aggregator.Properties[memberName] = binding.CreateScriptExpression();
            }


            protected void AssignProperty<J, T>(Expression<Func<J, T>> expression, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") where J : new() {
                var aggregator = (Aggregator.Current as Reactive);
                if (aggregator == null)
                    throw new InvalidOperationException($"{nameof(Bindings<FinalJs>)} must be aggregated into {nameof(Reactive)} only");

                aggregator.Properties[memberName] = new BindingScriptifier(expression, new J()).Eval();
               
            }
        }

        [ConstructorJs("ReactiveUtils")]
        [ConstructorJs("Math")]
        [ConstructorJs("Constants")]
        [ConstructorJs("Constructor")]
        [ConstructorJs("Bindings")]
        public abstract class Reactive : CallerInfo {
            public Dictionary<string, string> Properties { get; } = new();


            public string this[string propertyName] {
                get {
                    return Properties[propertyName];
                }

                set {
                    Properties[propertyName] = value;
                }
            }
            




            protected Reactive(Reactive other,
                string callerFilePath = "",
                int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
                
                Properties = new(other.Properties);
            }

            public Reactive(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) {
                
            }

            //public abstract Task<Tag> GenerateHtmlAsync(Context context);


            string FindScriptRoot() {
                foreach (var i in GetBaseTypes()) {
                    var attributes = i.GetCustomAttributes<ConstructorJsAttribute>(false);
                    foreach (var attribute in attributes) {
                        if (attribute.FileName == null) { // Script for class. Not aditional
                            return i.Name;
                        }
                    }
                }
                throw new Exception("There is no class in hierarchy, with related js component script");
            }

            private IEnumerable<Type> GetBaseTypes() {
                var type = GetType();
                while (type != null) {
                    yield return type;
                    if (type == typeof(Reactive))
                        yield break;
                    type = type.BaseType;
                }
            }

            private async Task AddRequiredIncluesForType(Type type, Context context) {
                if (type != typeof(Reactive)) {
                    var baseType = type.BaseType;
                    if (baseType != null) {
                        await AddRequiredIncluesForType(baseType, context);
                    }
                }

                var assembly = type.Assembly;
                var scriptsAttributes = type.GetCustomAttributes<ConstructorJsAttribute>(false);
                var typeName = type.Name;

                foreach (var i in scriptsAttributes) {
                    string directory = Path.GetDirectoryName(i.CallerFilePath) ?? "";
                    var fileName = i.FileName ?? typeName;
                    var extension = i.Extension;

                    string absoluteFilePath = Path.GetFullPath(Path.Combine(directory, fileName+ extension));

                    if (File.Exists(absoluteFilePath)) {
                        var scriptFromFile = await (new FileGenome(absoluteFilePath)).CreateOrGetCached();
                        context.AddScript(scriptFromFile);

                    } else {
                        var relativeFilePath = AssemblyResourcesUtils.GetFilePathRelativeToProject(assembly, absoluteFilePath);
                        var relativeResourcePath = AssemblyResourcesUtils.GetResourcePath(relativeFilePath);

                        var script = await (new AssemblyResourceGenome(assembly, relativeResourcePath)).CreateOrGetCached();
                        context.AddScript(script);
                    }

                    
                }
            }

            public virtual async Task AddRequiredInclues(Context context) {
                var type = GetType();
                await AddRequiredIncluesForType(type, context);

                /*await context.AddScriptFromResource("ReactiveUtils.js");
                await context.AddScriptFromResource("Math.js");
                await context.AddScriptFromResource("Constants.js");
                await context.AddScriptFromResource("Constructor.js");
                await context.AddScriptFromResource("Bindings.js");*/
            }



            public async Task<Tag> CreateConstructorScriptAsync(Context context) {
                var sripts = new List<string>();


                var className = FindScriptRoot();

                var propertiesInitializers = await GetGeneratedBundingsAsync().ToListAsync();
                propertiesInitializers.AddRange(Properties);

                var propertiesInitializersScript = string.Join(',', propertiesInitializers.Select(x => $"{x.Key}:{x.Value}"));

                string script = $"{{let element = Constructor(\"{className}\");";
                if (!string.IsNullOrEmpty(propertiesInitializersScript)) {
                    script += $"element.Reactive={{{propertiesInitializersScript}}}";
                }
                script += "}";

                return new Tag("script") {
                    new PureHtmlNode(script)
                };
            }

            public virtual IAsyncEnumerable<KeyValuePair<string, string>> GetGeneratedBundingsAsync() {
                return AsyncEnumerable.Empty<KeyValuePair<string, string>>();
            }


        }
    }
    
}