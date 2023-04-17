using Microsoft.VisualStudio.Shell.Interop;
using Scopes;
using Scopes.C;

using StaticSharp.Gears;
using StaticSharp.Html;
using StaticSharp.Js;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StaticSharp {


    


    /*[Javascriptifier.JavascriptClass("")]
    public static class HierarchicalStatic {

        [Javascriptifier.JavascriptMethodFormat("{0}.First(x=>x.Name=={1})")]
        public static T ByName<T>(this Enumerable<T> enumerable, string name) where T : JHierarchical {
            throw new Javascriptifier.JavascriptOnlyException();
        }
    }*/


    public interface JHierarchical : JEntity {
        public string Name { get; set; }
        public bool Exists { get; set; }
        public JPage Root { get; }
        public JHierarchical Parent { get; }

    }

    namespace Gears {
        public class SocketAttribute : Attribute { 
        }    
    }


    namespace Js {
        public class Variable<T> : Javascriptifier.IStringifiable where T: JEntity {

            [Javascriptifier.JavascriptPropertyGetFormat("{0}")]
            [Javascriptifier.JavascriptOnlyMember]
            public T Value => throw new Javascriptifier.JavascriptOnlyException();
            public string Name { get; set; }
            public Variable([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
                Name = "_"+Hash.CreateFromString($"{callerLineNumber}\0{callerFilePath}").ToString(8);
            }
            public string ToJavascriptString() {
                return Name;
            }
        }
    }




    [Scripts.Layer]
    [ConstructorJs]
    [RelatedScript("DomLinkedList")]
    //[Mix(typeof(AssignMixin<Hierarchical, Js.Hierarchical>))]
    public abstract partial class Hierarchical : Entity {
        protected virtual string TagName => CaseUtils.CamelToKebab(GetType().Name);

        /*public virtual Hierarchical Assign(out Js.Variable<JHierarchical> variable, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")// where T : Hierarchical where JsT : Js.Hierarchical 
        {
            variable = new(callerLineNumber, callerFilePath);
            return Assign(variable);
        }
        public Hierarchical Assign(Js.Variable<JHierarchical> variable) {
            if (VariableNames == null) VariableNames = new();
            VariableNames.Add(variable.Name);
            return this;
        } */      

        protected string TagToJsValue(Tag tag) {
            return TagToJsValue(tag.Id);
        }
        protected string TagToJsValue(string id) {
            return $"""document.getElementById("{id}")""";
        }
        public virtual void ModifyTagAndScript(Context context, Tag tag, Scopes.Group script) { 
            
        }


        public virtual TagAndScript Generate(Context context) {

            context = ModifyContext(context);

            var type = GetType();
            var id = context.CreateId();

            var result = new TagAndScript(new Tag(TagName,id), new Group());

            var jsConstructorsNames = FindJsConstructorsNames();

            var scriptOfCurrentElement = new Group() {
                $"let {id} = {TagToJsValue(id)}",
                VariableNames?.Select(x=>$"let {x} = {id}"),
                jsConstructorsNames.Select(x=>$"{x}({id})")                
            };

            if (Properties.Count > 0) {
                scriptOfCurrentElement.Add(new Scope($"{id}.Reactive = "){
                    Properties.Select(x => $"{x.Key}:{x.Value},")
                });
            }

            AddSourceCodeNavigationData(result.Tag, context);

            ModifyTagAndScript(context, result.Tag, scriptOfCurrentElement);
            

            var properties = type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance).Where(x=>x.CanRead);
            var sockets = properties.Where(x=>x.GetCustomAttribute<SocketAttribute>()!=null).ToArray();

            foreach (var property in sockets) { 
                var value = property.GetValue(this);
                
                if (value == null)
                    continue;

                if (value is Hierarchical hierarchical) {
                    var item = hierarchical.Generate(context);
                    result.Tag.Add(item.Tag);
                    result.Script.Add(item.Script);
                    scriptOfCurrentElement.Add($"{id}.{property.Name} = {item.Tag.Id}");
                    continue;
                }

                if (value is Blocks blocks) {
                    var collectionName = property.Name;
                    var items = blocks.Select(x=>x.Generate(context)).ToArray();
                    if (items.Length == 0) { continue; }

                    if (items.Length > 0) {

                        foreach (var item in items) {
                            result.Tag.Add(item.Tag);
                            result.Script.Add(item.Script);
                        }

                        scriptOfCurrentElement.Add($"{id}.{collectionName}First = {items[0].Tag.Id}");
                        for (int i = 1; i< items.Length; i++) {
                            var p = items[i-1];
                            var c = items[i];
                            scriptOfCurrentElement.Add($"{p.Tag.Id}.NextSibling = {c.Tag.Id}");
                        }
                    }
                    continue;
                }
            }

            //scriptOfCurrentElement.Add($"if (typeof ({id}.AfterChildren) === \"function\") {{ {id}.AfterChildren() }}");

            result.Script.Add(scriptOfCurrentElement);
            return result;
        }




    }

}

