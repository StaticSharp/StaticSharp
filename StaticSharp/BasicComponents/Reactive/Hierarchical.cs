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

            public MBlockJs ParentBlock => NotEvaluatableObject<MBlockJs>();

            public T Sibling<T>(string id) where T :  new() => NotEvaluatableObject<T>();
            public HierarchicalJs Sibling(string id) => NotEvaluatableObject<HierarchicalJs>();
            public T Child<T>(string id) where T :  new() => NotEvaluatableObject<T>();

        }

        public class HierarchicalBindings<FinalJs> : MBindings<FinalJs> where FinalJs : new() {

        }

    }


    [RelatedScript]
    public abstract class Hierarchical : Reactive {

        protected virtual string TagName => "div";
        
        
        protected Hierarchical(Hierarchical other,
            string callerFilePath,
            int callerLineNumber) : base(other, callerFilePath, callerLineNumber) {

            
        }

        public Hierarchical(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

  
        

    }

}

