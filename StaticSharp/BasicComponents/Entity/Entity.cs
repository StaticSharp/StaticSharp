using EnvDTE;
using NUglify.Helpers;
using Scopes;
using Scopes.C;
using StaticSharp.Gears;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace StaticSharp {
    public struct Binding<J, T> {

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

        public static implicit operator Binding<J, T>(T value) {
            return new Binding<J, T>(value);
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

    public class TempruaryId {
        private static ulong CurrentId = 0;
        private ulong Id = CurrentId++;

        public override string ToString() {
            return $"<{Id}>";
        }
    }


    public class Property<J, T> : Entity.Property, Javascriptifier.IStringifiable {
        private Expression<Func<J, T>> Expression;

        public Property(Expression<Func<J, T>> expression) {
            Expression = expression;
        }

        /*public Property<J, T> Attach(Entity entity) {
            this.entity = entity;
            return this;
        }*/

        public string ToJavascriptString() {
            return tempruaryId.ToString();
        }

        [Javascriptifier.JavascriptOnlyMember]
        [Javascriptifier.JavascriptPropertyGetFormat("{0}")]

        public T Front => throw new Javascriptifier.JavascriptOnlyException();

        

        /*public static implicit operator T(Property<J, T> value) {
            throw new Javascriptifier.JavascriptOnlyException();
        }*/

        public override string Script => Javascriptifier.ExpressionScriptifier.Scriptify(Expression);
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
    public abstract partial class Entity : CallerInfo, Javascriptifier.IStringifiable {
        
        protected List<Property> UserProperties { get; } = new();
        public abstract class Property {

            public TempruaryId tempruaryId { get; } = new();

            protected Entity? entity = null;
            
            public Entity? Entity {
                set {
                    if (entity != null) {
                        entity.UserProperties.Remove(this);
                    }
                    if (value == null) return;
                    entity = value;
                    entity.UserProperties.Add(this);
                }
            }

            public abstract string Script { get; }
        }

        //todo: move to generator
        public Entity AttachProperty(Property property) {
            property.Entity = this;
            return this;
        }

        public Entity CreateProperty<T>(out Property<JEntity, T> property, Expression<Func<JEntity, T>> expression) {
            property = new(expression);
            AttachProperty(property);
            return this;
        }
        public Property<JEntity, T> CreateProperty<T>(Expression<Func<JEntity, T>> expression) {
            var result = new Property<JEntity, T>(expression);
            AttachProperty(result);
            return result;
        }


        protected TempruaryId tempruaryId = new();
        public string ToJavascriptString() {
            return tempruaryId.ToString();
        }


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

        [Javascriptifier.JavascriptOnlyMember]
        [Javascriptifier.JavascriptPropertyGetFormat("{0}")]
        public JEntity Front => throw new Javascriptifier.JavascriptOnlyException();

        public string FrontVariableName => tempruaryId.ToString();



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

        protected virtual IEnumerable<Genome<IAsset>> GetGeneratedScripts() { 
            return Enumerable.Empty<Genome<IAsset>>();
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


        protected void AddPropertiesToScript(string id, Group script, Context context) {
            if (Properties.Count == 0 && UserProperties.Count == 0)
                return;

            var scope = new Scope($"{id}.Reactive = ");

            foreach (var i in Properties) {
                scope.Add($"{i.Key}:{i.Value},");
            }

            int userPropertyIndex = 0;
            foreach (var i in UserProperties) {
                var tempruaryId = i.tempruaryId;
                context.TemporaryIdToId.Add(new(
                    tempruaryId.ToString(),
                    $"{id}.UserProperty{userPropertyIndex}"
                    ));
                scope.Add($"UserProperty{userPropertyIndex}:{i.Script},");

                userPropertyIndex++;
                
            }


            script.Add(scope);
        }

    }


}