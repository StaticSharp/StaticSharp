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


    public class ScriptBeforeAttribute : Attribute {}
    public class ScriptAfterAttribute : Attribute { }


    

    namespace Gears {

        


        public abstract class Reactive<Js> : CallerInfo where Js : ObjectJs, new() {

            //public interface IBinding { }
            public struct Binding<T> {
                public string? Script { get; } = null;
                public Binding() { }
                public Binding(string script) {
                    Script = script;
                }
                public static implicit operator Binding<T>(string script) {
                    return new Binding<T>(script);
                }
                public Binding(T value) {
                    var currentCulture = Thread.CurrentThread.CurrentCulture;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    try {
                        Script = value?.ToString();
                    }
                    finally {
                        Thread.CurrentThread.CurrentCulture = currentCulture;
                    }
                }

                public static implicit operator Binding<T>(T value) {
                    return new Binding<T>(value);
                }
                public Binding(Expression<Func<Js, T>> expression) {
                    Script = new BindingScriptifier(expression, new Js()).Eval();
                }
                public static implicit operator Binding<T>(Func<Js, T> expression) {
                    return new Binding<T>("");
                }
                public override string? ToString() {
                    return Script;
                }
            }

            

            //public delegate T Binding<out T>(Js element);

            //public Dictionary<string, Binding<SymbolJs>> Properties { get; private set; } = new();
            public Reactive(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

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


            public Tag CreateScriptBefore() {
                var className = FindScriptRoot<ScriptBeforeAttribute>(GetType());
                return new Tag("script") {
                new PureHtmlNode($"ConstructorBefore(\"{className}\",\'{PropertiesInitializationScript()}\')")
            };
            }

            public Tag CreateScriptAfter() {
                var className = FindScriptRoot<ScriptAfterAttribute>(GetType());
                return new Tag("script") {
                new PureHtmlNode($"ConstructorAfter(\"{className}\")")
            };
            }


            public IEnumerable<KeyValuePair<string,string>> GetBindings() {
                foreach (var i in GetType().GetProperties()) {
                    MethodInfo? getter = i.GetGetMethod(nonPublic: true);
                    if (i.CanRead) {
                        if (i.PropertyType.IsGenericType)
                            if (i.PropertyType.GetGenericTypeDefinition() == typeof(Binding<>)) {
                                var value = i.GetValue(this)?.ToString();
                                if (value != null) {
                                    yield return new(i.Name,value);
                                }                            
                            }
                    }
                }
            } 


            public string PropertiesInitializationScript() {

                var bindings = GetBindings().ToArray();
                if (bindings.Length == 0)
                    return "";

                return $"element.Reactive={{{string.Join(',', bindings.Select(x=>$"{x.Key}:{x.Value}"))}}}";


                /*StringBuilder stringBuilder = new("element.Reactive = {");
                foreach (var binding in bindings) {
                
                }
                stringBuilder.Append("}");

                var initializationScript = (this as IReactiveObjectCs).ToJsObject(new Js() { value = "element" });
                if (string.IsNullOrEmpty(initializationScript))
                    return "";
                return "element.Reactive = " + initializationScript;*/
            }

        }
    }
    
}