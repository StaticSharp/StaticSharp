
namespace Exo.RoslynSourceGeneratorDebuggable {
    internal interface IAbstractSourceGenerator {
        void Execute(IGeneratorExecutionContext context);
        void Initialize(IGeneratorInitializationContext context);
    }
}
