using StaticSharp;
using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaticSharp {


    namespace Js {
        public interface Object {
            public Object this[string name] { get; }
        }

    }

    namespace Js {
        public interface Hierarchical : Object {
            public string Id { get; }

            public Page Root { get; }
            public Hierarchical Parent { get; }
            //public Block ParentBlock { get; }


            /*public Hierarchical Sibling(string id) => NotEvaluatableObject<Hierarchical>();
            public T Sibling<T>(string id) where T : new() => NotEvaluatableObject<T>();







            public T Child<T>(int id) where T : new() => NotEvaluatableObject<T>();
            public Hierarchical Child(string id) => NotEvaluatableObject<Hierarchical>();
            public T Child<T>(string id) where T : new() => NotEvaluatableObject<T>();*/

        }



    }


    namespace Gears {
        public class HierarchicalBindings<FinalJs> : Bindings<FinalJs> {

        }
    }



    [ConstructorJs]
    public abstract class Hierarchical : Reactive {
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
        public virtual Tag GenerateHtml(Context context, Role? role) {

            AddRequiredInclues(context);

            context = ModifyContext(context);

            var tag = new Tag(TagName) { };

            role?.SetAttributes(tag);

            //ModifyTag(tag);

            AddSourceCodeNavigationData(tag, context);

            tag.Add(CreateConstructorScript(context));

            ModifyHtml(context, tag);

            tag.Add(Pop());

            return tag;
        }

    }

}

