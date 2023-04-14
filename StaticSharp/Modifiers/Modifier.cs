using Scopes;
using Scopes.C;
using StaticSharp.Gears;

namespace StaticSharp {

    public interface JModifier : JEntity {
        bool Enabled { get; set; }
    }




    [ConstructorJs]
    public abstract partial class Modifier : Entity {

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


