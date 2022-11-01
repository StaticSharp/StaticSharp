using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        public class Hierarchical : Object {
            public string Id => NotEvaluatableValue<string>();

            public Page Root => NotEvaluatableObject<Page>();
            public Hierarchical Parent => NotEvaluatableObject<Hierarchical>();
            public Block ParentBlock => NotEvaluatableObject<Block>();

            //public T GetParent<T>() where T : new() => NotEvaluatableObject<T>();

            public Hierarchical Sibling(string id) => NotEvaluatableObject<Hierarchical>();
            public T Sibling<T>(string id) where T :  new() => NotEvaluatableObject<T>();


            




            public T Child<T>(int id) where T : new() => NotEvaluatableObject<T>();
            public Hierarchical Child(string id) => NotEvaluatableObject<Hierarchical>();
            public T Child<T>(string id) where T :  new() => NotEvaluatableObject<T>();

        }

        

    }


    namespace Gears {
        public class HierarchicalBindings<FinalJs> : Bindings<FinalJs> where FinalJs : new() {

        }
    }



    [ConstructorJs]
    public abstract class Hierarchical : Reactive {
        protected virtual string TagName => CaseUtils.CamelToKebab(GetType().Name);        
        
        protected Hierarchical(Hierarchical other,
            string callerFilePath,
            int callerLineNumber) : base(other, callerFilePath, callerLineNumber) {            
        }
        public Hierarchical(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

        protected virtual Context ModifyContext(Context context) {
            return context;
        }
        protected virtual Task ModifyHtmlAsync(Context context, Tag elementTag) {
            return Task.CompletedTask;
        }
        public virtual async Task<Tag> GenerateHtmlAsync(Context context) {

            await AddRequiredInclues(context);

            context = ModifyContext(context);

            var tag = new Tag(TagName) { };

            //ModifyTag(tag);

            AddSourceCodeNavigationData(tag, context);

            tag.Add(await CreateConstructorScriptAsync(context));

            await ModifyHtmlAsync(context, tag);

            tag.Add(Pop());

            return tag;
        }

    }

}

