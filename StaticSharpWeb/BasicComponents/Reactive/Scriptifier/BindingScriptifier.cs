using StaticSharp.Gears;
using System;
using System.Linq.Expressions;

namespace StaticSharp;

public class BindingScriptifier : LambdaScriptifier {

    public object ParameterValue { get; }
    public string ParameterName { get; }

    public BindingScriptifier(LambdaExpression expression, object parameterValue, string parameterName = "element"): base(expression) {

        if (ParametersExpressions.Length != 1) {
            throw new InvalidOperationException("BindingScriptifier expression must have exactly 1 parameter");
        }

        ParameterValue = parameterValue;
        ParameterName = parameterName;
    }

    protected override string ReplaceParameterName(string name) {
        return ParameterName;
    }

    protected override object[] GetParametersValues() {
        return new object[] { ParameterValue };
    }

    public override string Eval() {
        var result = $"() => {Eval(LambdaExpression.Body)}";
        return result;
    }


}
