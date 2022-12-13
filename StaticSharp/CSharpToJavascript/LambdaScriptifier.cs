using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace StaticSharp.Gears;

public class LambdaScriptifier {
    public Expression Body { get; }

    public record Parameter(ParameterExpression ParameterExpression, object Value) {}

    public Parameter[] Parameters { get; }
    public Parameter[]? InheritedParameters { get; } = null;

    /*public ParameterExpression[] ParametersExpressions { get; }
    public object[] ParametersValues { get; }*/

    public LambdaScriptifier(LambdaExpression expression, object[] parametersValues) {
        Body = expression.Body;
        var parametersExpressions = expression.Parameters.ToArray();
        Parameters = Enumerable.Range(0, parametersValues.Length).Select(i => new Parameter(parametersExpressions[i], parametersValues[i])).ToArray();

        
        //ParametersValues = parametersValues;
    }

    public LambdaScriptifier(Expression body, Parameter[] parameters, Parameter[] inheritedParameters) {
        Body = body;
        Parameters = parameters;
        InheritedParameters = inheritedParameters;
    }


    /*protected virtual object GetParameterValue(string name) {
       return ObjectJs.NotEvaluatable<object>();
    }*/
    protected virtual string ReplaceParameterName(string name) => name;
    //private IEnumerable<string> ParametersNames => ParametersExpressions.Select(x => ReplaceParameterName(x.Name));
    /*protected virtual object[] GetParametersValues() {
        return Enumerable.Range(0, ParametersExpressions.Length).Select(x => (object)null!).ToArray();
    }*/
    public virtual string Eval() {
        var result = $"({string.Join(',', Parameters.Select(x=>x.ParameterExpression.Name))}) => {Eval(Body)}";
        //var result = $"({string.Join(',', ParametersNames)}) => {Eval(LambdaExpression.Body)}";
        return result;
    }


    private object CreateNotEvaluatableValue(Type type) {
        return Activator.CreateInstance(type);
    }

    protected string Eval(Expression expression) {
        

        /*if (expression is LambdaExpression lambdaExpression) {
            return new LambdaScriptifier(lambdaExpression, ParametersValues).Eval();
        }*/

        if (expression is ParameterExpression parameterExpression) {
            return parameterExpression.Name;// ReplaceParameterName(parameterExpression.Name);
        }

        if (expression is UnaryExpression unaryExpression) {
            
            if (unaryExpression.NodeType == ExpressionType.Quote) {
                var lambdaExpression = unaryExpression.Operand as LambdaExpression;

                //var inheritedParameters = Parameters.ToList();
                var parameters = lambdaExpression.Parameters.Select(x => new Parameter(x, CreateNotEvaluatableValue(x.Type)));

                //currentParameters.AddRange(newParameters);
                var script = new LambdaScriptifier(lambdaExpression.Body, parameters.ToArray(), Parameters).Eval();

                return script;
            }
            if (unaryExpression.NodeType == ExpressionType.Convert) {
                return Eval(unaryExpression.Operand);
            }

        }

        var allParameters = Parameters.ToList();
        if (InheritedParameters!=null)
            allParameters.AddRange(InheritedParameters);

        var lambda = Expression.Lambda(expression, allParameters.Select(x=>x.ParameterExpression));


        var compiled = lambda.Compile();

        Js.Static.NotEvaluatableFound = false;
        var value = compiled.DynamicInvoke(allParameters.Select(x => x.Value).ToArray());
        if (Js.Static.NotEvaluatableFound) {
            var result = Stringify(expression);
            return result;
        } else {
            return CSValueToJSValueConverter.ObjectToJsValue(value);
        }

        /*var result = Stringify(expression);
        return result;*/
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

            //var classConverter = expression.Method.DeclaringType?.GetCustomAttribute<ConvertToJsAttribute>()?.Format;

            return $"{expression.Method.DeclaringType.Name}.{expression.Method.Name}({string.Join(',', argumentsValues)})";
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
                    throw NotImplemented(unaryExpression);
                    //return Eval(unaryExpression.Operand);
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

            /*case ConstantExpression constantExpression: {
                return CSValueToJSValueConverter.ObjectToJsValue(constantExpression.Value);
            }*/


            default:
            throw NotImplemented(expression);
        }

    }



}
