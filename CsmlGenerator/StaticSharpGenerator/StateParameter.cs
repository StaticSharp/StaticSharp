using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharpGenerator {
    class StateParameter {
        public string Name { get; private set; }
        public string ParameterName { get; private set; }
        public ITypeSymbol Type { get; private set; }
        public string TypeName { get; private set; }

        public StateParameter(string parameterName, ITypeSymbol type, string typeName) {
            char[] a = parameterName.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            Name = new string(a);

            ParameterName = parameterName;
            Type = type;
            TypeName = typeName;
        }        
    }

    static class StateParametersStatic {


        public static string ToCall(this StateParameter[] _this) {
            return $"{string.Join(",", _this.Select(x => x.Name))}";
        }

        public static string ToBaseCall(this StateParameter[] _this) {
            return $"{string.Join(",", _this.Select(x => x.ParameterName))}";
        }

        public static string ToRecordParametersDeclaration(this StateParameter[] _this) {
            if (_this.Length == 0) return "";
            return $"({string.Join(",", _this.Select(x => $"{x.TypeName} {x.Name}"))})";
        }
        public static string ToConstructorParameters(this StateParameter[] _this) {
            return $"{string.Join(",", _this.Select(x => $"{x.TypeName} {x.ParameterName}"))}";
        }

        /*
        public static StateParameter[] MakeCompatibleState(this StateParameter[] _this, Compilation compilation, INamedTypeSymbol symbol) {
            
            var constructors = symbol.Constructors.Where(x => x.Parameters.Length <= _this.Length).OrderByDescending(x => x.Parameters.Length);
            foreach (var c in constructors) {
                var result = _this.MakeCompatibleState(compilation, c);
                if (result != null)
                    return result;
            }
            return null;
        }

        public static StateParameter[] MakeCompatibleState(this StateParameter[] _this, Compilation compilation, IMethodSymbol constructor) {

            var result = constructor.Parameters.Select(paremeter => {
                for (int j = 0; j < _this.Length; j++) {
                    if (paremeter.Name == _this[j].Name) {
                        var conversion = compilation.ClassifyConversion(_this[j].Type, paremeter.Type);
                        if (conversion.IsIdentity) {
                            return _this[j];
                        }
                    }
                }
                return null;
            }).ToArray();

            if (result.Any(x => x == null)) return null;

            return result;

        }*/


    }
}