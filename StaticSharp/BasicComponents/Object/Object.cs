

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
        public class IdAndScript {
            public Scopes.Group? Script { get; init; }
            public string Id { get; init; }

            public IdAndScript(string id, Group? script) {
                Id = id;
                Script = script;
                
            }
        }

        public class TagAndScript { 
            public Scopes.Group? Script { get; init; }
            public Tag Tag { get; init; }            

            public TagAndScript(Tag tag, Group? script) {
                Script = script;
                Tag = tag;
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

            protected List<string>? VariableNames;

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

            protected virtual Context ModifyContext(Context context) {
                AddRequiredInclues(context);
                return context;
            }

        }
    }

}