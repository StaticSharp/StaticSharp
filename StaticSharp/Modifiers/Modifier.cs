using Scopes;
using Scopes.C;

namespace StaticSharp {

    namespace Js {
        public interface Modifier {
            bool Enabled { get; }
        }
    }
    namespace Gears {
        public class ModifierBindings<FinalJs> : Bindings<FinalJs> {
            public Binding<bool> Enabled { set { Apply(value); } }
        }
    }


    namespace Gears {

        [ConstructorJs]
        [Mix(typeof(ModifierBindings<Js.Modifier>))]
        public abstract class Modifier : Object {
            protected Modifier(int callerLineNumber, string callerFilePath) : base(callerLineNumber, callerFilePath) {
            }

            protected Modifier(Object other, int callerLineNumber = 0, string callerFilePath = "") : base(other, callerLineNumber, callerFilePath) {
            }

            public virtual IdAndScript Generate(string elementId, Context context) {

                context = ModifyContext(context);

                var id = context.CreateId();

                

                var jsConstructorsNames = FindJsConstructorsNames();
                if (jsConstructorsNames.Length != 1) {
                    throw new Exception("Modifiers must have exactly one constructor");
                }

                var script = new Group() {
                    $"let {id} = new {jsConstructorsNames[0]}({elementId})",
                    VariableNames?.Select(x=>$"let {x} = {id}"),
                };

                if (Properties.Count > 0) {
                    script.Add(new Scope($"{id}.Reactive = "){
                        Properties.Select(x => $"{x.Key}:{x.Value},")
                    });
                }

                return new IdAndScript(id, script);
            }
        }
    }


}


