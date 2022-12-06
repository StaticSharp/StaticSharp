using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    /*protected virtual object GetParameterValue(string name) {
       return ObjectJs.NotEvaluatable<object>();
    }*/
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





        /*try {*/

        if (expression is LambdaExpression lambdaExpression) {
            return new LambdaScriptifier(lambdaExpression).Eval();
        }

        if (expression is ParameterExpression parameterExpression) {
            return ReplaceParameterName(parameterExpression.Name);
        }

        Stopwatch stopwatch = Stopwatch.StartNew();

        var compiled = lambda.Compile();

        Js.Static.NotEvaluatableFound = false;
        var value = compiled.DynamicInvoke(GetParametersValues());
        if (Js.Static.NotEvaluatableFound) {
            var result = Stringify(expression);
            return result;
        } else {
            return CSValueToJSValueConverter.ObjectToJsValue(value);
        }

    }



    private string StringifyMethodCall(MethodCallExpression expression) {
        var parameters = expression.Method.GetParameters();
        var arguments = expression.Arguments.ToArray();
        List<string> argumentsValues = new();

        //expression.Object.Type.GetDefaultMembers

        //Console.WriteLine(expression.Method.Name);


        for (int i = 0; i < arguments.Length; i++) {

            var isParams = parameters[i].GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0;
            if (isParams) {
                if (arguments[i] is NewArrayExpression newArrayExpression) {
                    foreach (var e in newArrayExpression.Expressions) {
                        argumentsValues.Add(Eval(e));
                    }
                } else {
                    NotImplemented(arguments[i]);
                }
            } else {
                argumentsValues.Add(Eval(arguments[i]));
            }
        }

        var customConverter = expression.Method.GetCustomAttribute<ConvertToJsAttribute>()?.Format;

        if (customConverter != null) {
            if (expression.Object != null) {
                return string.Format(customConverter, argumentsValues.Prepend(Eval(expression.Object)).ToArray());
            }
            return string.Format(customConverter, argumentsValues.ToArray());

        } else {

            if (expression.Object != null) {
                return $"{Eval(expression.Object)}.{expression.Method.Name}({string.Join(',', argumentsValues)})";
            }

            return $"{expression.Method.Name}({string.Join(',', argumentsValues)})";
        }


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
                var prefix = "";
                if (memberExpression.Expression == null) {//Static class
                    if (memberExpression.Member.DeclaringType != null) {
                        var format = memberExpression.Member.DeclaringType.GetCustomAttribute<ConvertToJsAttribute>()?.Format;
                        if (format != null)
                            prefix = format + ".";
                        else
                            prefix = memberExpression.Member.DeclaringType.Name + ".";
                    } else {
                        throw new NotImplementedException();
                    }
                } else {
                    prefix = Eval(memberExpression.Expression) + ".";
                }
                return prefix + memberExpression.Member.Name;
            }

            case UnaryExpression unaryExpression: {
                if (expression.NodeType == ExpressionType.Convert) {
                    //TODO: implement smarter
                    return Eval(unaryExpression.Operand);


                    /*if (unaryExpression.Operand is ParameterExpression parameterExpression) {
                        var resultType = expression.Type;
                        if (parameterExpression.Type == typeof(NotEvaluatable<>).MakeGenericType(resultType)) {
                            return parameterExpression.Name;
                        }
                    } else {
                        return Eval(unaryExpression.Operand);

                    }*/
                }

                var Op = unaryExpression.NodeType switch {
                    ExpressionType.UnaryPlus => "+",
                    ExpressionType.Negate => "-",
                    ExpressionType.Not => "!",
                    _ => throw NotImplemented(expression)
                };
                return $"({Op}{Eval(unaryExpression.Operand)})";
            }

            case ConditionalExpression conditionalExpression: {
                return $"({Eval(conditionalExpression.Test)}?{Eval(conditionalExpression.IfTrue)}:{Eval(conditionalExpression.IfFalse)})";
            }

            case BinaryExpression binaryExpression: {

                if (binaryExpression.Method != null) {
                    var customConverter = binaryExpression.Method.GetCustomAttribute<ConvertToJsAttribute>()?.Format;
                    if (customConverter != null) {
                        return string.Format(customConverter, Eval(binaryExpression.Left), Eval(binaryExpression.Right));
                    }
                }


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
