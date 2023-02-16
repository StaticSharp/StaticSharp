

using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace StaticSharp {
    namespace Gears {

        public class Bindings<FinalJs> {
            public class Binding<T> : IVoidEnumerable {

                private T? Value;
                private Expression<Func<FinalJs, T>>? Expression;


                public Binding(Expression<Func<FinalJs, T>> expression) {
                    Expression = expression;
                }
                public Binding(T value) {
                    Value = value;
                }

                public static implicit operator Binding<T>(T value) {
                    return new Binding<T>(value);
                }
                public string CreateScriptExpression() {
                    if (Expression != null) {
                        return Javascriptifier.ExpressionScriptifier.EvalLambdaExpression(Expression).ToString();
                        //return new LambdaScriptifier(Expression,new object[] { new FinalJs()}).Eval();
                    }
                    return Javascriptifier.ValueStringifier.Stringify(Value);
                    //return CSValueToJSValueConverter.ObjectToJsValue(Value);
                }
            }

            protected void Apply<T>(Binding<T> binding,
                [System.Runtime.CompilerServices.CallerMemberName] string? memberName = null,
                string? memberName1 = null,
                string? memberName2 = null,
                string? memberName3 = null) {

                var memberNames = new string?[] { memberName, memberName1, memberName2, memberName3 };

                var aggregator = (Aggregator.Current as Reactive);
                if (aggregator == null)
                    throw new InvalidOperationException($"{nameof(Bindings<FinalJs>)} must be aggregated into {nameof(Reactive)} only");

                foreach (var i in memberNames) {
                    if (i != null) {
                        aggregator.Properties[i] = binding.CreateScriptExpression();
                    }
                }
            }
        }




        [RelatedScript("ReactiveUtils")]
        [RelatedScript("../../CrossplatformLibrary/Math/Math")]
        [RelatedScript("../../CrossplatformLibrary/Linq/Linq")]
        [RelatedScript("Constants")]
        [RelatedScript("Constructor")]
        [RelatedScript("Bindings")]
        [RelatedScript("Events")]
        public abstract class Reactive : CallerInfo {
            public Dictionary<string, string> Properties { get; } = new();

            public string this[string propertyName] {
                /*get {
                    return Properties[propertyName];
                }*/

                set {
                    Properties[propertyName] = value;
                }
            }


            protected Reactive(Reactive other,
                int callerLineNumber = 0,
                string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
                Properties = new(other.Properties);
            }

            protected Reactive(int callerLineNumber, string callerFilePath) : base(callerLineNumber, callerFilePath) {

            }

            //public abstract Task<Tag> GenerateHtmlAsync(Context context);


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
                    if (type == typeof(Reactive))
                        yield break;
                    type = type.BaseType;
                }
            }

            private void AddRequiredIncluesForType(Type type, Context context) {
                if (type != typeof(Reactive)) {
                    var baseType = type.BaseType;
                    if (baseType != null) {
                        AddRequiredIncluesForType(baseType, context);
                    }
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



            protected Tag CreateConstructorScript(Context context) {
                var jsConstructorsNames = FindJsConstructorsNames();

                var propertiesInitializers = GetGeneratedBundings(context).ToList();
                propertiesInitializers.AddRange(Properties);

                var propertiesInitializersScript = string.Join(',', propertiesInitializers.Select(x => $"{x.Key}:{x.Value}"));

                string script = $"{{let element = Constructor({string.Join(',', jsConstructorsNames)});";
                if (!string.IsNullOrEmpty(propertiesInitializersScript)) {
                    script += $"element.Reactive={{{propertiesInitializersScript}}}";
                }
                script += "}";

                return new Tag("script") {
                    new PureHtmlNode(script)
                };
            }




            protected Tag CreateScript(string code) {
                return new Tag("script") {
                    new PureHtmlNode(code)
                };
            }

            protected Tag CreateScript_SetCurrentSocket(string name) {
                return new Tag("script") {
                    new PureHtmlNode($"SetCurrentSocket(\"{name}\")")
                };
            }

            protected Tag CreateScript_AssignToParentProperty(string name) {
                return new Tag("script") {
                    new PureHtmlNode($"AssignToParentProperty(\"{name}\")")
                };
            }

            protected Tag CreateScript_AssignPreviousTagToParentProperty(string name) {
                return new Tag("script") {
                    new PureHtmlNode($"AssignPreviousTagToParentProperty(\"{name}\")")
                };
            }

            protected virtual IEnumerable<KeyValuePair<string, string>> GetGeneratedBundings(Context context) {
                return Enumerable.Empty<KeyValuePair<string, string>>();
            }


        }
    }

}