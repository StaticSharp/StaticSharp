using StaticSharp.Gears;
using System.Reflection;

namespace StaticSharp {


    public interface JEntity {
        //public JEntity this[string name] { get; }
    }


    [Scripts.ReactiveUtils]
    [Scripts.Num]
    [Scripts.Linq]
    [Scripts.Animation]

    [RelatedScript("Constructor")]
    [RelatedScript("Bindings")]
    [RelatedScript("Events")]
    [Scripts.TypeCast]
    [ConstructorJs]
    public abstract class Entity : CallerInfo {
        public Dictionary<string, string> Properties { get; } = new();

        protected List<string>? VariableNames;

        public string this[string propertyName] {
            /*get {
                return Properties[propertyName];
            }*/

            set {
                Properties[propertyName] = value;
            }
        }

        protected Entity(Entity other,
            int callerLineNumber = 0,
            string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Properties = new(other.Properties);
        }

        protected Entity(int callerLineNumber, string callerFilePath) : base(callerLineNumber, callerFilePath) {

        }

        protected string[] FindJsConstructorsNames() {
            foreach (var i in GetBaseTypes()) {
                var attributes = i.GetCustomAttributes<ConstructorJsAttribute>(false);
                if (attributes.Any()) {

                    return attributes.Select(x => string.IsNullOrEmpty(x.ClassName) ? i.Name : x.ClassName).ToArray();
                }
            }
            throw new Exception($"{nameof(ConstructorJsAttribute)} not found for {GetType().FullName}");
        }

        private IEnumerable<Type> GetBaseTypes() {
            var type = GetType();
            while (type != null) {
                yield return type;
                if (type == typeof(Entity))
                    yield break;
                type = type.BaseType;
            }
        }

        private void AddRequiredIncluesForType(Type type, Context context) {
            if (type != typeof(Entity)) {
                var baseType = type.BaseType;
                if (baseType != null) {
                    AddRequiredIncluesForType(baseType, context);
                }
            }

            foreach (var i in type.GetCustomAttributes<Scripts.ScriptReferenceAttribute>(false)) {
                context.AddScript(i.GetGenome());
            }

            foreach (var i in type.GetCustomAttributes<RelatedScriptAttribute>(false)) {
                context.AddScript(i.GetGenome(type));
            }
            foreach (var i in type.GetCustomAttributes<RelatedStyleAttribute>(false)) {
                context.AddStyle(i.GetGenome(type));
            }

        }

        protected virtual void AddRequiredInclues(Context context) {
            var type = GetType();
            AddRequiredIncluesForType(type, context);
        }

        protected virtual Context ModifyContext(Context context) {
            AddRequiredInclues(context);
            return context;
        }

    }


}