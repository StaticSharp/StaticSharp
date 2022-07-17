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

            public Hierarchical(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

            public override void AddRequiredInclues(IIncludes includes) {
                base.AddRequiredInclues(includes);
                includes.Require(new Script(AbsolutePath("Hierarchical.js")));
            }

            public virtual Task<Tag?> GenerateHtmlChildrenAsync(Context context) {
                throw new System.NotImplementedException($"{GetType().FullName} overrides nither GenerateHtmlChildrenAsync nor GenerateHtmlAsync");
            }

            public virtual async Task<Tag> GenerateHtmlAsync(Context context, string? id = null) {
                AddRequiredInclues(context.Includes);
                foreach (var m in Modifiers) {
                    m.AddRequiredInclues(context.Includes);
                    context = m.ModifyContext(context);
                }

                var tag = new Tag(TagName, id) {
                    CreateScriptInitialization(),
                    Modifiers.Select(x=>x.CreateScriptInitialization()),

                    CreateScriptBefore(),
                    Modifiers.Select(x=>x.CreateScriptBefore()),

                    await GenerateHtmlChildrenAsync(context),

                    Modifiers.Select(x=>x.CreateScriptAfter()).Reverse(),
                    CreateScriptAfter()
                };

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