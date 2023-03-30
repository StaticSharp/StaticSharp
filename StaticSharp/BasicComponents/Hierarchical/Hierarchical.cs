using AngleSharp.Dom;
using StaticSharp;
using StaticSharp.Gears;
using StaticSharp.Html;
using StaticSharp.Js;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace StaticSharp {


    


    [Javascriptifier.JavascriptClass("")]
    public static class HierarchicalStatic {

        [Javascriptifier.JavascriptMethodFormat("{0}.First(x=>x.Name=={1})")]
        public static T ByName<T>(this Enumerable<T> enumerable, string name) where T : Js.Hierarchical {
            throw new Javascriptifier.JavascriptOnlyException();
        }
    }


    namespace Js {



        
        public interface Hierarchical : Object {
            public string Name { get; }
            public bool Exists { get; }
            public Page Root { get; }
            public Hierarchical Parent { get; }
            //public Hierarchical FirstChild { get; }
            public Hierarchical NextSibling { get; }
            public Js.Enumerable<Hierarchical> Siblings { get; }
            public Js.Enumerable<Hierarchical> UnmanagedChildren { get; }
        }
    }


    namespace Gears {
        public class HierarchicalBindings<FinalJs> : Bindings<FinalJs> {
            public Binding<string> Name { set { Apply(value); } }
            public Binding<bool> Exists { set { Apply(value); } }
        }
    }

    namespace Gears {
        public class SocketAttribute : Attribute { 
        }
    
    }



    [Scripts.Layer]
    [ConstructorJs]
    [RelatedScript("DomLinkedList")]
    public abstract class Hierarchical : Gears.Object {
        protected virtual string TagName => CaseUtils.CamelToKebab(GetType().Name);

        protected Hierarchical(Hierarchical other,
            int callerLineNumber,
            string callerFilePath) : base(other, callerLineNumber, callerFilePath) {
        }
        public Hierarchical(int callerLineNumber, string callerFilePath) : base(callerLineNumber, callerFilePath) { }

        protected virtual Context ModifyContext(Context context) {
            return context;
        }
        protected virtual void ModifyHtml(Context context, Tag elementTag) {
        }








        public virtual Tag GenerateHtml(Context context) {

            /*var type = GetType();
            var properties = type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);

            var sockets = properties.Where(x=>x.GetCustomAttribute<SocketAttribute>()!=null).ToArray();

            var singleSockets = sockets.Where(x => x.PropertyType.IsAssignableTo(typeof(Hierarchical)));                

            var listSockets = sockets.Where(x => x.PropertyType.IsAssignableTo(typeof(Blocks)));

            if (listSockets.Count() != 0) {
                Console.WriteLine("Sockets");
            }*/

            AddRequiredInclues(context);

            context = ModifyContext(context);

            var tag = new Tag(TagName) { };


            //ModifyTag(tag);

            AddSourceCodeNavigationData(tag, context);

            tag.Add(CreateConstructorScript());

            ModifyHtml(context, tag);

            tag.Add(CreateScript("Pop()"));

            return tag;
        }

    }

}

