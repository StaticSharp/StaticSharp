using Scopes;
using Scopes.C;
using StaticSharp.Gears;
using StaticSharp.Html;

namespace StaticSharp {

    public interface JModifier : JEntity {
        bool Enabled { get; set; }
    }




    [ConstructorJs]
    public abstract partial class Modifier : Entity {

        public virtual string? TagRename => null;


        public virtual IdAndScript Generate(Tag element, Context context) {

            context = ModifyContext(context);

            var modifierId = context.CreateId();



            var jsConstructorsNames = FindJsConstructorsNames();
            if (jsConstructorsNames.Length != 1) {
                throw new Exception("Modifiers must have exactly one constructor");
            }

            var script = new Group() {
                    $"let {modifierId} = {{}}",
                    $"{jsConstructorsNames[0]}({modifierId},{element.Id})",
                    VariableNames?.Select(x=>$"let {x} = {modifierId}"),
                };

            AddPropertiesToScript(modifierId,script,context);

            return new IdAndScript(modifierId, script);
        }
    }



}


