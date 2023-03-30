

using Scopes;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StaticSharp {


    namespace Js {
        public interface Object {
            public Object this[string name] { get; }
        }
    }


    namespace Gears {


        public class ScriptfullTag: Tag { 
            
        
        }

        public class Element2 { 
            public Scopes.Group? InitializationScript { get; init; }
            public Tag Tag { get; init; }            

            public Element2(Tag tag, Group? initializationScript = null) {
                InitializationScript = initializationScript;
                Tag = tag;
            }

            public static Element2 Create(string className, string id, string? tagName = null) {
                if (tagName == null)
                    tagName = CaseUtils.CamelToKebab(className);
                return new Element2(new Tag(tagName, id), new Group());
            }

            public static Element2 CreateScriptless(string tagName, string id) {
                return new Element2(new Tag(tagName, id));
            }


        }




        [Scripts.ReactiveUtils]
        [Scripts.Math]
        [Scripts.Linq]

        [RelatedScript("Constructor")]
        [RelatedScript("Bindings")]
        [RelatedScript("Events")]
        public abstract class Object : CallerInfo {
            public Dictionary<string, string> Properties { get; } = new();

            public string this[string propertyName] {
                /*get {
                    return Properties[propertyName];
                }*/

                set {
                    Properties[propertyName] = value;
                }
            }


            protected Object(Object other,
                int callerLineNumber = 0,
                string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
                Properties = new(other.Properties);
            }

            protected Object(int callerLineNumber, string callerFilePath) : base(callerLineNumber, callerFilePath) {

            }

            //public abstract Task<Tag> GenerateHtmlAsync(Context context);


            protected string[] FindJsConstructorsNames() {
                foreach (var i in GetBaseTypes()) {
                    var attributes = i.GetCustomAttributes<ConstructorJsAttribute>(false);
                    if (attributes.Any()) {

                        return attributes.Select(x => string.IsNullOrEmpty(x.ClassName) ? i.Name : x.ClassName).ToArray();
                    }
                }
                throw new Exception($"{nameof(ConstructorJsAttribute)} not found for {GetType().FullName}");
            }

            private IEnumerable<Type> GetBaseTypes() {
                var type = GetType();
                while (type != null) {
                    yield return type;
                    if (type == typeof(Object))
                        yield break;
                    type = type.BaseType;
                }
            }

            private void AddRequiredIncluesForType(Type type, Context context) {
                if (type != typeof(Object)) {
                    var baseType = type.BaseType;
                    if (baseType != null) {
                        AddRequiredIncluesForType(baseType, context);
                    }
                }

                foreach (var i in type.GetCustomAttributes<Scripts.ScriptReferenceAttribute>(false)) {
                    context.AddScript(i.GetGenome());
                }

                foreach (var i in type.GetCustomAttributes<RelatedScriptAttribute>(false)) {
                    context.AddScript(i.GetGenome(type));
                }
                foreach (var i in type.GetCustomAttributes<RelatedStyleAttribute>(false)) {
                    context.AddStyle(i.GetGenome(type));
                }

            }

            protected virtual void AddRequiredInclues(Context context) {
                var type = GetType();
                AddRequiredIncluesForType(type, context);
            }



            protected Tag CreateConstructorScript() {
                var jsConstructorsNames = FindJsConstructorsNames();

                var propertiesInitializersScript = string.Join(',', Properties.Select(x => $"{x.Key}:{x.Value}"));

                string script = $"{{let element = Constructor({string.Join(',', jsConstructorsNames)});";
                if (!string.IsNullOrEmpty(propertiesInitializersScript)) {
                    script += $"element.Reactive={{{propertiesInitializersScript}}}";
                }
                script += "}";

                return new Tag("script") {
                    new PureHtmlNode(script)
                };
            }




            protected Tag CreateScript(string code) {
                return new Tag("script") {
                    new PureHtmlNode(code)
                };
            }

            protected Tag CreateScript_SetCurrentSocket(string name) {
                return new Tag("script") {
                    new PureHtmlNode($"SetCurrentSocket(\"{name}\")")
                };
            }

            protected Tag CreateScript_SetCurrentCollectionSocket(string name) {
                return CreateScript_SetCurrentSocket(name + "First");
            }

            protected Tag CreateScript_AssignToParentProperty(string name) {
                return new Tag("script") {
                    new PureHtmlNode($"AssignToParentProperty(\"{name}\")")
                };
            }

            protected Tag CreateScript_AssignPreviousTagToParentProperty(string name) {
                return new Tag("script") {
                    new PureHtmlNode($"AssignPreviousTagToParentProperty(\"{name}\")")
                };
            }
        }
    }

}