using StaticSharp.Animations;
using System.Linq.Expressions;

namespace StaticSharp {

    public static partial class Static {
        public static Expression<Func<Js, T>> Animate<Js, T>(Js js, Expression<Func<Js, T>> func) {
            return func;
        }
    }

    namespace Animations {

        public abstract class BindingWrapper { 
        
        }
        public class Linear : BindingWrapper {
            public Linear(double duration) { 
            
            }
        }

        


    }

    namespace Js.BindingWrappers {
        class Animation { 
            
        }
    }




    /*namespace Gears {


        public class Bindings<FinalJs> {
            public struct Binding<T> : IVoidEnumerable {

                private T? Value;
                private Expression<Func<FinalJs, T>>? Expression;
                private List<LambdaExpression>? bindingWrappers = null;

                public double Animation { set { } }

                public Binding(Expression<Func<FinalJs, T>> expression) {
                    Expression = expression;
                }
                public Binding(T value) {
                    Value = value;
                }

                public static implicit operator Binding<T>(T value) {
                    return new Binding<T>(value);
                }

                public Binding<T> this[Expression index] {
                    set {
                        // set the instance value at index
                    }
                }


                public void Add<W>(Expression<Func<FinalJs, W>> wrapper) {
                    bindingWrappers ??= new List<LambdaExpression>();

                    bindingWrappers.Add(wrapper);
                }


                public string CreateScriptExpression() {
                    if (Expression != null) {
                        var script = Javascriptifier.ExpressionScriptifier.Scriptify(Expression).ToString();
                        return script;
                    }
                    return Javascriptifier.ValueStringifier.Stringify(Value);
                }

                

            }

            protected void Apply<T>(Binding<T> binding,
                [System.Runtime.CompilerServices.CallerMemberName] string? memberName = null,
                string? memberName1 = null,
                string? memberName2 = null,
                string? memberName3 = null) {

                var memberNames = new string?[] { memberName, memberName1, memberName2, memberName3 };

                var aggregator = (Aggregator.Current as Entity);
                if (aggregator == null)
                    throw new InvalidOperationException($"{nameof(Bindings<FinalJs>)} must be aggregated into {nameof(Entity)} only");

                foreach (var i in memberNames) {
                    if (i != null) {
                            aggregator.Properties[i] = binding.CreateScriptExpression();
                    }
                }
            }
        }
    }*/

}