using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StaticSharp {

    public class ScriptBeforeAttribute : Attribute {}
    public class ScriptAfterAttribute : Attribute { }


    

    namespace Gears {



        public abstract class Reactive<Js> : CallerInfo, IReactiveObjectCs where Js : ObjectJs, new() {
            
            public delegate T Binding<out T>(Js element);


            

            

            public Reactive(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

            //public abstract Task<Tag> GenerateHtmlAsync(Context context);

            public virtual void AddRequiredInclues(IIncludes includes) {
                includes.Require(new Script(AbsolutePath("ReactiveUtils.js")));
                includes.Require(new Script(AbsolutePath("Math.js")));
                includes.Require(new Script(AbsolutePath("Constants.js")));
                includes.Require(new Script(AbsolutePath("Constructor.js")));
                includes.Require(new Script(AbsolutePath("Bindings.js")));
            }

            public string GeneratePropertyDeclaration() {
                return GetType().FullName;
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

            public string PropertiesInitializationScript() {
                var initializationScript = (this as IReactiveObjectCs).ToJsObject(new Js() { value = "element" });
                if (string.IsNullOrEmpty(initializationScript))
                    return "";
                return "element.Reactive = " + initializationScript;
            }

        }
    }
    
}