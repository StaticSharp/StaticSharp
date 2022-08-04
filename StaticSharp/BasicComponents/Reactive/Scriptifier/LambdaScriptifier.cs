using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace StaticSharp.Gears;

public class LambdaScriptifier {
    public LambdaExpression LambdaExpression { get; }
    public ParameterExpression[] ParametersExpressions { get; }
    public LambdaScriptifier(LambdaExpression expression) {
        LambdaExpression = expression;
        ParametersExpressions = expression.Parameters.ToArray();
    }
    protected virtual object GetParameterValue(string name) {
        throw new NotEvaluatableException();
    }
    protected virtual string ReplaceParameterName(string name) => name;
    private IEnumerable<string> ParametersNames => ParametersExpressions.Select(x => ReplaceParameterName(x.Name));
    protected virtual object[] GetParametersValues() {
        return Enumerable.Range(0, ParametersExpressions.Length).Select(x => (object)null!).ToArray();
    }
    public virtual string Eval() {
        var result = $"({string.Join(',', ParametersNames)}) => {Eval(LambdaExpression.Body)}";
        return result;
    }    
    protected string Eval(Expression expression) {
        var lambda = Expression.Lambda(expression, ParametersExpressions);
        var compiled = lambda.Compile();
        try {

            if (expression is LambdaExpression lambdaExpression) {
                return new LambdaScriptifier(lambdaExpression).Eval();
            }

            if (expression is ParameterExpression parameterExpression) {
                return ReplaceParameterName(parameterExpression.Name);
            }

            var value = compiled.DynamicInvoke(GetParametersValues());
            return Static.ObjectToJsValue(value);
        }
        catch (TargetInvocationException ex) {
            if (ex.InnerException != null && (ex.InnerException is NotEvaluatableException)) {
                return Stringify(expression);
            } else
                throw;
        }
    }

    

    private string StringifyMethodCall(MethodCallExpression expression) {
        var arguments = expression.Arguments.ToArray();
        string[] argumentsValues = new string[arguments.Length];

        for (int i = 0; i < arguments.Length; i++) {
            argumentsValues[i] = Eval(arguments[i]);
        }

        if (expression.Object != null) {
            return $"{Eval(expression.Object)}.{expression.Method.Name}({string.Join(',', argumentsValues)})";
        }

        return $"{expression.Method.Name}({string.Join(',', argumentsValues)})";
    }


    private Exception NotImplemented(Expression expression) {
        return new NotImplementedException($"Expression Type: {expression.GetType().FullName} NodeType: {expression.NodeType}");
    }

    private string Stringify(Expression expression) {

        switch (expression) {
            case MethodCallExpression methodCallExpression: {
                    return StringifyMethodCall(methodCallExpression);
                }

            case MemberExpression memberExpression: {
                    return Eval(memberExpression.Expression) + "." + memberExpression.Member.Name;
                }

            case UnaryExpression unaryExpression: {
                    if (expression.NodeType == ExpressionType.Convert) {
                        if (unaryExpression.Operand is ParameterExpression parameterExpression) {
                            var resultType = expression.Type;
                            if (parameterExpression.Type == typeof(NotEvaluatable<>).MakeGenericType(resultType)) {
                                return parameterExpression.Name;
                            }
                        } else {
                            return Eval(unaryExpression.Operand);

                        }
                    }
                    throw NotImplemented(expression);
                }

            case ConditionalExpression conditionalExpression: {
                    return $"({Eval(conditionalExpression.Test)}?{Eval(conditionalExpression.IfTrue)}:{Eval(conditionalExpression.IfFalse)})";
                }

            case BinaryExpression binaryExpression: {
                    var Op = binaryExpression.NodeType switch {
                        ExpressionType.Add => "+",
                        ExpressionType.Subtract => "-",
                        ExpressionType.Multiply => "*",
                        ExpressionType.Divide => "/",
                        ExpressionType.Modulo => "%",

                        ExpressionType.And => "&",
                        ExpressionType.AndAlso => "&&",

                        ExpressionType.Or => "|",
                        ExpressionType.OrElse => "||",

                        ExpressionType.ExclusiveOr => "^",

                        ExpressionType.LessThan => "<",
                        ExpressionType.LessThanOrEqual => "<=",
                        ExpressionType.GreaterThan => ">",
                        ExpressionType.GreaterThanOrEqual => ">=",


                        ExpressionType.Equal => "==",
                        ExpressionType.NotEqual => "!=",
                        ExpressionType.Not => "!",

                        _ => throw NotImplemented(expression)
                    };
                    return $"({Eval(binaryExpression.Left)}{Op}{Eval(binaryExpression.Right)})";
                }

            default:
                throw NotImplemented(expression);
        }

    }



}
