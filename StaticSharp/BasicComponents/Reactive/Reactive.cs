using Microsoft.Extensions.FileProviders;
using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StaticSharp {

    //public class ScriptInitializationAttribute : Attribute { }
    public class ScriptBeforeAttribute : Attribute {}
    public class ScriptAfterAttribute : Attribute { }


    

    namespace Gears {


        [System.Diagnostics.DebuggerNonUserCode]
        public class ReactiveJs {
        }


        public class ReactiveBindings<FinalJs> where FinalJs : new() {
            private Dictionary<string, string> Properties;
            protected void AssignProperty<J, T>(Expression<Func<J, T>> expression, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") where J : new() {
                Properties[memberName] = new BindingScriptifier(expression, new J()).Eval();
            }
            public ReactiveBindings(Dictionary<string, string> properties) {
                Properties = properties;
            }
        }


        public abstract class Reactive : CallerInfo {
            public Dictionary<string, string> Properties { get; } = new();

            public ReactiveBindings<ReactiveJs> Bindings => new(Properties);

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

            public virtual void AddRequiredInclues(IIncludes includes) {
                includes.Require(new Script(AbsolutePath("ReactiveUtils.js")));
                includes.Require(new Script(AbsolutePath("Math.js")));
                includes.Require(new Script(AbsolutePath("Constants.js")));
                includes.Require(new Script(AbsolutePath("Constructor.js")));
                includes.Require(new Script(AbsolutePath("Bindings.js")));
            }

            string? FindScriptRoot<A>(Type? type) {
                if (type == null) return null;
                if (type.GetCustomAttributes(typeof(A), false).Length > 0) {
                    if (type.IsGenericType) {
                        return $"{type.Name.Substring(0, type.Name.IndexOf("`"))}";
                    }
                    return type.Name;
                }
                return FindScriptRoot<A>(type.BaseType);
            }

            public async Task<Tag> CreateScriptInitialization() {

                var className = FindScriptRoot<ScriptBeforeAttribute>(GetType());

                var propertiesInitializers = await GetGeneratedBundingsAsync().ToListAsync();
                propertiesInitializers.AddRange(Properties);

                var propertiesInitializersScript = string.Join(',', propertiesInitializers.Select(x => $"{x.Key}:{x.Value}"));

                string script = $"{{let element = ConstructorInitialization(\"{className}\");";
                if (!string.IsNullOrEmpty(propertiesInitializersScript)) {
                    script += $"element.Reactive={{{propertiesInitializersScript}}}";
                }
                script += "}";

                return new Tag("script") {
                    new PureHtmlNode(script)
                };
            }

            public Tag CreateScriptBefore() {
                var className = FindScriptRoot<ScriptBeforeAttribute>(GetType());
                    return new Tag("script") {
                    new PureHtmlNode($"ConstructorBefore(\"{className}\")")
                };
            }

            public Tag CreateScriptAfter() {
                var className = FindScriptRoot<ScriptAfterAttribute>(GetType());
                return new Tag("script") {
                    new PureHtmlNode($"ConstructorAfter(\"{className}\")")
                };
            }


            /*public IEnumerable<KeyValuePair<string,string>> GetBindings() {

                foreach (var i in GetType().GetProperties()) {
                    //MethodInfo? getter = i.GetGetMethod(nonPublic: true);
                    if (i.CanRead) {
                        if (i.PropertyType.IsGenericType) {
                            if (i.PropertyType.GetGenericTypeDefinition() == typeof(Binding<>)) {
                                var value = i.GetValue(this)?.ToString();
                                if (value != null) {
                                    yield return new KeyValuePair<string, string>(i.Name, value);
                                }
                            }
                        }
                    }
                }
            }*/

            public virtual IAsyncEnumerable<KeyValuePair<string, string>> GetGeneratedBundingsAsync() {
                return AsyncEnumerable.Empty<KeyValuePair<string, string>>();
            }

            private async Task<string> CombineGeneratedBindingsAsync() {
                var generatedBundings = await GetGeneratedBundingsAsync().ToArrayAsync();
                return string.Join(',', generatedBundings.Select(x => $"{x.Key}:{x.Value}"));
            }

            /*public string PropertiesInitializers() {
                var bindings = GetBindings().ToArray();


                if (bindings.Length == 0)
                    return "";

                return string.Join(',', bindings.Select(x=>$"{x.Key}:{x.Value}"));
            }*/

        }
    }
    
}