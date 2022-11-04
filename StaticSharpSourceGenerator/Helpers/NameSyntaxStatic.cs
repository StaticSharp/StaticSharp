using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

static class NameSyntaxStatic {

    private static int SplitPrivate(NameSyntax x, ref SimpleNameSyntax[] items, int count = 0) {
        if (x is SimpleNameSyntax simpleName) {
            items = new SimpleNameSyntax[count + 1];
            items[0] = simpleName;
            return count;
        }

        if (x is QualifiedNameSyntax qualifiedName) {
            var l = SplitPrivate(qualifiedName.Left, ref items, count + 1);
            items[l - count] = qualifiedName.Right;
            return l;
        }
        throw new NotImplementedException(x.GetType().Name);


    }

    public static SimpleNameSyntax[] Split(this NameSyntax x) {
        SimpleNameSyntax[] items = null;
        SplitPrivate(x, ref items);
        return items;
    }
}
