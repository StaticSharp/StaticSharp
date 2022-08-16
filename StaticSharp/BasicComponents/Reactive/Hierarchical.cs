﻿using StaticSharp.Gears;
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

    public class HierarchicalBindings<FinalJs> : ReactiveBindings<FinalJs> where FinalJs : new() {
        public HierarchicalBindings(Dictionary<string, string> properties) : base(properties) {
        }
    }



    namespace Gears {
        [RelatedScript]
        public abstract class Hierarchical : Reactive {

            public new HierarchicalBindings<HierarchicalJs> Bindings => new(Properties);
            public virtual string TagName => "div";
            public virtual List<Modifier> Modifiers { get; } = new();

            protected Hierarchical(Hierarchical other,
                string callerFilePath = "",
                int callerLineNumber = 0) : base(other, callerFilePath, callerLineNumber) {

                Modifiers = new(other.Modifiers);
            }

            public Hierarchical(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

            /*public override void AddRequiredInclues(Context context) {



                base.AddRequiredInclues(context);
                includes.Require(new Script(AbsolutePath("Hierarchical.js")));
            }*/

            protected virtual Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
                return Task.FromResult<Tag?>(null);
                //throw new System.NotImplementedException($"{GetType().FullName} overrides nither GenerateHtmlChildrenAsync nor GenerateHtmlAsync");
            }



            /*public virtual void ApplyModifiers(Tag tag) {
                foreach (var m in Modifiers) {
                    m.ModifyTag(tag);
                }
            }*/

            /*public virtual IEnumerable<Task<Tag>> CreateScripts(Context context) {
                yield return CreateScript(context);
                foreach (var m in Modifiers)
                    yield return m.CreateScript(context);

            }*/

            /*public virtual IEnumerable<Tag> After() {
                for (int i = Modifiers.Count-1; i >= 0; i--) {
                    yield return Modifiers[i].CreateScriptAfter();
                }
                yield return CreateScriptAfter();
            }*/


            public virtual async Task<Tag> GenerateHtmlAsync(Context context, string? id = null) {

                await AddRequiredInclues(context);

                foreach (var m in Modifiers) {
                    await m.AddRequiredInclues(context);
                    context = m.ModifyContext(context);
                }

                var tag = new Tag(TagName, id) {};

                foreach (var m in Modifiers) 
                    m.ModifyTag(tag);
                
                //tag.Add(await CreateScripts(context).SequentialOrParallel());

                tag.Add(await CreateScript(context));

                foreach (var m in Modifiers)
                    tag.Add(await m.CreateScript(context));


                tag.Add(await GenerateHtmlInternalAsync(context, tag));
                //tag.Add(After());

                return tag;
            }

        }

        /*public sealed class Hierarchical : Hierarchical<HierarchicalJs> {
            public Hierarchical([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }
        }*/
    }
    
}