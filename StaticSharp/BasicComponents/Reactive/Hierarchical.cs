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

        public BlockJs ParentBlock => throw new NotEvaluatableException();

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
                return Task.FromResult<Tag?>(null);
                //throw new System.NotImplementedException($"{GetType().FullName} overrides nither GenerateHtmlChildrenAsync nor GenerateHtmlAsync");
            }

            public virtual void ModifyContext(ref Context context) {
                foreach (var m in Modifiers) {
                    m.AddRequiredInclues(context.Includes);
                    context = m.ModifyContext(context);
                }
            }

            public virtual void ModifyTag(Tag tag) {
                foreach (var m in Modifiers) {
                    m.ModifyTag(tag);
                }
            }

            public virtual IEnumerable<Task<Tag>> Before() {
                yield return CreateScriptInitialization();
                foreach (var m in Modifiers)
                    yield return m.CreateScriptInitialization();

                yield return Task.FromResult(CreateScriptBefore());
                foreach (var m in Modifiers)
                    yield return Task.FromResult(m.CreateScriptBefore());
            }

            public virtual IEnumerable<Tag> After() {
                for (int i = Modifiers.Count-1; i >= 0; i--) {
                    yield return Modifiers[i].CreateScriptAfter();
                }
                yield return CreateScriptAfter();
            }


            public virtual async Task<Tag> GenerateHtmlAsync(Context context, string? id = null) {
                
                AddRequiredInclues(context.Includes);
                ModifyContext(ref context);

                var tag = new Tag(TagName, id) {};

                ModifyTag(tag);
                tag.Add(await Before().SequentialOrParallel());
                tag.Add(await GenerateHtmlInternalAsync(context, tag));
                tag.Add(After());

                return tag;
            }

        }

        /*public sealed class Hierarchical : Hierarchical<HierarchicalJs> {
            public Hierarchical([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }
        }*/
    }
    
}