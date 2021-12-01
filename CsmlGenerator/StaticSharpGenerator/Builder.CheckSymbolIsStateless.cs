using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharpGenerator {
    partial class Builder {
        readonly HashSet<string> knownStatelessTypes = new HashSet<string>();
        readonly HashSet<string> knownStatefulTypes = new HashSet<string>();


        bool CheckSymbolIsStateless(INamedTypeSymbol symbol) {
            if (symbol == null) return true;
            var name = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (knownStatelessTypes.Contains(name)) return true;
            if (knownStatefulTypes.Contains(name)) return false;
            var baseCheck = CheckSymbolIsStateless(symbol.BaseType);
            if (!baseCheck) {
                knownStatefulTypes.Add(name);
                return false;
            } else {
                var members = symbol.GetMembers();
                var containsFields = members.Any(m => m.Kind == SymbolKind.Field);
                if (containsFields) {
                    knownStatefulTypes.Add(name);
                    return false;
                } else {
                    knownStatelessTypes.Add(name);
                    return true;
                }
            }
        }
    }
}