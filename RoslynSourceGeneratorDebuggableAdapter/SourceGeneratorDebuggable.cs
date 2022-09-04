using Microsoft.CodeAnalysis;
using System;

namespace Exo.RoslynSourceGeneratorDebuggable {
    public abstract class SourceGeneratorDebuggable : IAbstractSourceGenerator, ISourceGenerator {
        public void Execute(GeneratorExecutionContext context) {
            Execute(new GeneratorExecutionContextWrapper(context));
        }
        public void Initialize(GeneratorInitializationContext context) {
            Initialize(new GeneratorInitializationContextWrapper(context));
        }
        public abstract void Initialize(IGeneratorInitializationContext context);
        public abstract void Execute(IGeneratorExecutionContext context);
    }
}