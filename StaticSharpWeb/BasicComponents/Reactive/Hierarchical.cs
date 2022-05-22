namespace StaticSharp {


    public class HierarchicalJs : ObjectJs {
        public HierarchicalJs() { }
        public HierarchicalJs(string value) : base(value) {
        }

        public SymbolJs Id => new($"{value}.Id");
        public HierarchicalJs Parent => new($"{value}.Parent");

        public HierarchicalJs ById(string id) => new($"{value}.ById(\"{id}\")");
    }
    


    namespace Gears {
        [ScriptBefore]
        [ScriptAfter]
        public abstract class Hierarchical<Js> : Reactive<Js> where Js : HierarchicalJs, new() {
            public Binding<StringJs> Id { set; protected get; } = null!;
            public Hierarchical(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

            public override void AddRequiredInclues(IIncludes includes) {
                base.AddRequiredInclues(includes);
                includes.Require(new Script(AbsolutePath("Hierarchical.js")));
            }


        }
    }
    
}