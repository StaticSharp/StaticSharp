using System;
using Microsoft.CodeAnalysis;
using RoutingSg.Src;

namespace RoutingSg {
    [Generator]
    public class RoutingSg : ISourceGenerator {

        public void Initialize(GeneratorInitializationContext context) {
            //Debugger.Launch();
        }

        //Dictionary<string, INamedTypeSymbol> knownValidTypes = new Dictionary<string, INamedTypeSymbol>();
        //Dictionary<string, INamedTypeSymbol> knownInvalidTypes = new Dictionary<string, INamedTypeSymbol>();

        /*public void Error(Location location, string messageFormat) {

            var descriptor = new DiagnosticDescriptor("id", "title", "messageFormat", "category", DiagnosticSeverity.Error, true);
            context.ReportDiagnostic(Diagnostic.Create(descriptor, location));

            ExecutionContext.ReportDiagnostic()
        }*/

        public void Execute(GeneratorExecutionContext context) {
            // This is needed because RoutingSg must not take effect on StaticSharp,
            // but must be referenced automatically by any project that references StaticSharp
            if (context.Compilation.Assembly.Name == "StaticSharp") {
                return;
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var builder = new PageTreeBuilder(context);
            var pagesTree = builder.Build();

            var classTree = ClassTreeWriter.Write(pagesTree, builder.State);
            var partials = PartialsWriter.Write(pagesTree, builder.State);

            context.AddSource("classTree.cs", classTree);
            context.AddSource("partials.cs", partials);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"Generator.Execute time: {elapsedMs} ms");
        }
    }
}