using StaticSharp;
using StaticSharp.Gears;
using StaticSharp.Html;
using StaticSharp.Js;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaticSharp {


    namespace Js {
        public interface Object {
            public Object this[string name] { get; }
        }

    }


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

            public Page Root { get; }
            public Hierarchical Parent { get; }
            public Hierarchical FirstChild { get; }
            public Hierarchical NextSibling { get; }
            public Js.Enumerable<Hierarchical> Children { get; }

        }



    }


    namespace Gears {
        public class HierarchicalBindings<FinalJs> : Bindings<FinalJs> {
            public Binding<string> Name { set { Apply(value); } }
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

            AddRequiredInclues(context);

            context = ModifyContext(context);

            var tag = new Tag(TagName) { };


            //ModifyTag(tag);

            AddSourceCodeNavigationData(tag, context);

            tag.Add(CreateConstructorScript(context));

            ModifyHtml(context, tag);

            tag.Add(CreateScript("Pop()"));

            return tag;
        }

    }

}

