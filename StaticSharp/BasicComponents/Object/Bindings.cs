using System.Linq.Expressions;

namespace StaticSharp {
    namespace Gears {
        public class Bindings<FinalJs> {
            public struct Binding<T> : IVoidEnumerable {

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

                var aggregator = (Aggregator.Current as Object);
                if (aggregator == null)
                    throw new InvalidOperationException($"{nameof(Bindings<FinalJs>)} must be aggregated into {nameof(Object)} only");

                foreach (var i in memberNames) {
                    if (i != null) {
                        /*if (binding == null)
                            aggregator.Properties[i] = Js.Constants.Undefined;
                        else*/
                            aggregator.Properties[i] = binding.CreateScriptExpression();
                    }
                }
            }
        }
    }

}