using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Gears {
        [System.Diagnostics.DebuggerNonUserCode]
        public class HierarchicalJs : ObjectJs {

            public string Id => NotEvaluatableValue<String>();
            public HierarchicalJs Parent => NotEvaluatableObject<HierarchicalJs>();

            public BlockJs ParentBlock => NotEvaluatableObject<BlockJs>();

            public T Sibling<T>(string id) where T : HierarchicalJs, new() => NotEvaluatableObject<T>();
            public HierarchicalJs Sibling(string id) => NotEvaluatableObject<HierarchicalJs>();
            public T Child<T>(string id) where T : HierarchicalJs, new() => NotEvaluatableObject<T>();

        }

        public class HierarchicalBindings<FinalJs> : ReactiveBindings<FinalJs> where FinalJs : new() {
            public HierarchicalBindings(Dictionary<string, string> properties) : base(properties) {
            }
        }

    }


    [RelatedScript]
    public abstract class Hierarchical : Reactive {

        public new HierarchicalBindings<HierarchicalJs> Bindings => new(Properties);
        protected virtual string TagName => "div";
        
        
        protected Hierarchical(Hierarchical other,
            string callerFilePath = "",
            int callerLineNumber = 0) : base(other, callerFilePath, callerLineNumber) {

            
        }

        public Hierarchical(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

  
        

    }

}

