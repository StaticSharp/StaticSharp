using Scopes;
using Scopes.C;
using StaticSharp.Gears;

namespace StaticSharp {

    public interface JModifier : JEntity {
        
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

            AddPropertiesToScript(id,script,context);

            return new IdAndScript(id, script);
        }
    }



}


