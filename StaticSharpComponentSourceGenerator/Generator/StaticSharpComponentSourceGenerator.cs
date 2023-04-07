using Microsoft.CodeAnalysis;
using System;

namespace StaticSharpComponentSourceGenerator
{
    [Microsoft.CodeAnalysis.Generator]
    public class StaticSharpComponentSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("Generated.cs", $@"
public static class HelloClass
{{
    public static string HelloString = ""Hello from {nameof(StaticSharpComponentSourceGenerator)}!"";
}}
");
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            
        }
    }
}
