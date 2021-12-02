using Microsoft.CodeAnalysis;
using System;

static class AccessibilityStatic {

    public static string ToCSharpName(this Accessibility x) {
        switch (x) {
            case Accessibility.Private:
                return "private";
            case Accessibility.ProtectedAndInternal:
                return "protected internal";
            case Accessibility.Protected:
                return "protected";            
            case Accessibility.Internal:
                return "internal";            
            case Accessibility.Public:
                return "public";
        }
        throw new NotImplementedException($"Accessibility is {x.ToString()}");
    }
}
