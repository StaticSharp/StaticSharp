using Microsoft.VisualStudio.Shell.Interop;
using Scopes;
using Scopes.C;

using StaticSharp.Gears;
using StaticSharp.Html;
using StaticSharp.Js;
using System.Globalization;
using System.Reflection;

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


    [Scripts.Layer]
    [ConstructorJs]
    [RelatedScript("DomLinkedList")]
    public abstract partial class Hierarchical : Entity {
        protected virtual string TagName => CaseUtils.CamelToKebab(GetType().Name);

        protected string TagToJsValue(Tag tag) {
            return TagToJsValue(tag.Id);
        }
        protected string TagToJsValue(string id) {
            return $"""document.getElementById("{id}")""";
        }

        protected static void SetTagName(Tag tag, string name) {
            if (tag.Name != null) {
                throw new Exception($"Tag name conflict \"{tag.Name}\" \"{name}\"");
            }
            tag.Name = name;
        }

        public virtual void ModifyTagAndScript(Context context, Tag tag, Scopes.Group script) { 
            
        }

        public virtual TagAndScript Generate(Context context) {

            context = ModifyContext(context);

            var type = GetType();

            var id = context.CreateId(tempruaryId.ToString());

            //Tag name will be set later
            var result = new TagAndScript(new Tag(null,id), new Group());

            var jsConstructorsNames = FindJsConstructorsNames();

            var scriptOfCurrentElement = new Group() {
                $"let {id} = {TagToJsValue(id)}",
                VariableNames?.Select(x=>$"let {x} = {id}"),
                jsConstructorsNames.Select(x=>$"{x}({id})")                
            };

            AddPropertiesToScript(id,scriptOfCurrentElement,context);

            AddSourceCodeNavigationData(result.Tag, context);

            ModifyTagAndScript(context, result.Tag, scriptOfCurrentElement);

            if (result.Tag.Name == null) {
                result.Tag.Name = TagName;
            }


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

