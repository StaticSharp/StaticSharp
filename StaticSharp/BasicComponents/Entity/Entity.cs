using StaticSharp.Gears;
using System.Linq.Expressions;
using System.Reflection;

namespace StaticSharp {
    public struct Binding<J,T> : IVoidEnumerable {

        public T? Value;
        public Expression<Func<J, T>>? Expression;
        //private List<LambdaExpression>? bindingWrappers = null;


        public Binding(Expression<Func<J, T>> expression) {
            Expression = expression;
        }
        public Binding(T value) {
            Value = value;
        }

        public bool IsExpression => Expression != null;

        public static implicit operator Binding<J,T>(T value) {
            return new Binding<J,T>(value);
        }

        /*public Binding<J, T> this[Expression index] {
            set {
                // set the instance value at index
            }
        }*/


        /*public void Add<W>(Expression<Func<J, W>> wrapper) {
            bindingWrappers ??= new List<LambdaExpression>();

            bindingWrappers.Add(wrapper);
        }*/


        public string CreateScriptExpression() {
            if (Expression != null) {
                var script = Javascriptifier.ExpressionScriptifier.Scriptify(Expression).ToString();
                return script;
            }
            return Javascriptifier.ValueStringifier.Stringify(Value);
        }
    }


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
    public abstract partial class Entity : CallerInfo {
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
            int callerLineNumber,
            string callerFilePath) : base(callerLineNumber, callerFilePath) {
            Properties = new(other.Properties);
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