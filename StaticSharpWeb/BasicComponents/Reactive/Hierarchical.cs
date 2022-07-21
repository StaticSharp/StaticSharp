using StaticSharp.Gears;
using StaticSharp.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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

            public virtual string TagName => "div";
            public virtual List<Modifier> Modifiers { get; } = new();

            protected Hierarchical(Hierarchical<Js> other,
                string callerFilePath = "",
                int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {

                Modifiers = new(other.Modifiers);
            }

            public Hierarchical(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

            public override void AddRequiredInclues(IIncludes includes) {
                base.AddRequiredInclues(includes);
                includes.Require(new Script(AbsolutePath("Hierarchical.js")));
            }

            public virtual Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
                throw new System.NotImplementedException($"{GetType().FullName} overrides nither GenerateHtmlChildrenAsync nor GenerateHtmlAsync");
            }

            public virtual async Task<Tag> GenerateHtmlAsync(Context context, string? id = null) {
                AddRequiredInclues(context.Includes);
                foreach (var m in Modifiers) {
                    m.AddRequiredInclues(context.Includes);
                    context = m.ModifyContext(context);
                }

                var tag = new Tag(TagName, id);



                tag.Add(CreateScriptInitialization());
                tag.Add(Modifiers.Select(x=>x.CreateScriptInitialization()));
                tag.Add(CreateScriptBefore());
                tag.Add(Modifiers.Select(x=>x.CreateScriptBefore()));

                tag.Add(await GenerateHtmlInternalAsync(context, tag));

                tag.Add(Modifiers.Select(x=>x.CreateScriptAfter()).Reverse());
                tag.Add(CreateScriptAfter());


                foreach (var m in Modifiers) { 
                    m.ModifyTag(tag);
                }

                return tag;
            }

        }

        /*public sealed class Hierarchical : Hierarchical<HierarchicalJs> {
            public Hierarchical([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }
        }*/
    }
    
}