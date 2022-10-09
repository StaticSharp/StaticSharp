using Microsoft.Extensions.FileProviders;

using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StaticSharp {


    namespace Gears {



        public class Bindings<FinalJs> where FinalJs : new() {
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
                        return new BindingScriptifier(Expression, new FinalJs()).Eval();
                    }
                    return Static.ObjectToJsValue(Value);
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

            /*protected void Apply<T>(Binding<T> binding, params string[] memberNames) {


            }*/



        }

        [RelatedScript("ReactiveUtils")]
        [RelatedScript("Math")]
        [RelatedScript("Constants")]
        [RelatedScript("Constructor")]
        [RelatedScript("Bindings")]
        [RelatedScript("Events")]
        public abstract class Reactive : CallerInfo {
            public Dictionary<string, string> Properties { get; } = new();


            public string this[string propertyName] {
                get {
                    return Properties[propertyName];
                }

                set {
                    Properties[propertyName] = value;
                }
            }
            




            protected Reactive(Reactive other,
                string callerFilePath = "",
                int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
                
                Properties = new(other.Properties);
            }

            public Reactive(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) {
                
            }

            //public abstract Task<Tag> GenerateHtmlAsync(Context context);


            string[] FindJsConstructorsNames() {
                foreach (var i in GetBaseTypes()) {
                    var attributes = i.GetCustomAttributes<ConstructorJsAttribute>(false);
                    if (attributes.Any()) {

                        return attributes.Select(x=> string.IsNullOrEmpty(x.ClassName)? i.Name: x.ClassName).ToArray();
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

            private async Task AddRequiredIncluesForType(Type type, Context context) {
                if (type != typeof(Reactive)) {
                    var baseType = type.BaseType;
                    if (baseType != null) {
                        await AddRequiredIncluesForType(baseType, context);
                    }
                }

                foreach (var i in type.GetCustomAttributes<RelatedScriptAttribute>(false)) {
                    context.AddScript(await i.GetAssetAsync(type));
                }
                foreach (var i in type.GetCustomAttributes<RelatedStyleAttribute>(false)) {
                    context.AddStyle(await i.GetAssetAsync(type));
                }

            }

            public virtual async Task AddRequiredInclues(Context context) {
                var type = GetType();
                await AddRequiredIncluesForType(type, context);

                /*await context.AddScriptFromResource("ReactiveUtils.js");
                await context.AddScriptFromResource("Math.js");
                await context.AddScriptFromResource("Constants.js");
                await context.AddScriptFromResource("Constructor.js");
                await context.AddScriptFromResource("Bindings.js");*/
            }



            public async Task<Tag> CreateConstructorScriptAsync(Context context) {
                var sripts = new List<string>();


                var jsConstructorsNames = FindJsConstructorsNames();


                var propertiesInitializers = await GetGeneratedBundingsAsync(context).ToListAsync();
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

            public virtual IAsyncEnumerable<KeyValuePair<string, string>> GetGeneratedBundingsAsync(Context context) {
                return AsyncEnumerable.Empty<KeyValuePair<string, string>>();
            }


        }
    }
    
}