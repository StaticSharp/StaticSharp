using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

static class ITypeSymbolStatic {
    public static IEnumerable<ISymbol> GetMembersFlattenHierarchy(this ITypeSymbol x) {
        
        if (x.BaseType != null) {
            var baseMembers = x.BaseType.GetMembersFlattenHierarchy().ToList();
            var thisMembers = x.GetMembers();
            if (baseMembers.Count == 0) return thisMembers;

            foreach (var m in thisMembers) {
                baseMembers.RemoveAll(i => i.Name == m.Name);
            }            
            return thisMembers.AddRange(baseMembers);
        }

        return x.GetMembers();
    }

    public static bool IsPartial(this ITypeSymbol x) {
        var firstSyntaxReferences = x.DeclaringSyntaxReferences.FirstOrDefault();
        if (firstSyntaxReferences==null) return false;

        if (firstSyntaxReferences.GetSyntax() is TypeDeclarationSyntax declarationSyntax) {
            return declarationSyntax.IsPartial();
        }
        return false;
        //return x.DeclaringSyntaxReferences.Skip(1).Any();
    }

}


//INamespaceOrTypeSymbol