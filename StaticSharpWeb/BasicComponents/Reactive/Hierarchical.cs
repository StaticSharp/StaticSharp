using StaticSharp.Gears;
using System;

namespace StaticSharp {

    [System.Diagnostics.DebuggerNonUserCode]
    public class HierarchicalJs : ObjectJs {

        public string Id => throw new NotEvaluatableException();
        public HierarchicalJs Parent => throw new NotEvaluatableException();

        public T Sibling<T>(string id) where T : HierarchicalJs => throw new NotEvaluatableException();
        public T Child<T>(string id) where T : HierarchicalJs => throw new NotEvaluatableException();

    }



    namespace Gears {
        [ScriptBefore]
        [ScriptAfter]
        public abstract class Hierarchical<Js> : Reactive<Js> where Js : HierarchicalJs, new() {
            
            public Hierarchical(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

            public override void AddRequiredInclues(IIncludes includes) {
                base.AddRequiredInclues(includes);
                includes.Require(new Script(AbsolutePath("Hierarchical.js")));
            }


        }
    }
    
}