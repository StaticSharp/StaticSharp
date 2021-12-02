using Microsoft.CodeAnalysis;
using System;
using System.Threading;

class GeneratorInitializationContextWrapper : IGeneratorInitializationContext {

    public GeneratorInitializationContext GeneratorInitializationContext { get; private set; }

    public GeneratorInitializationContextWrapper(GeneratorInitializationContext generatorInitializationContext) {
        GeneratorInitializationContext = generatorInitializationContext;
    }

    public CancellationToken CancellationToken => GeneratorInitializationContext.CancellationToken;

    public void RegisterForPostInitialization(Action<GeneratorPostInitializationContext> callback) => GeneratorInitializationContext.RegisterForPostInitialization(callback);

    public void RegisterForSyntaxNotifications(SyntaxReceiverCreator receiverCreator) => GeneratorInitializationContext.RegisterForSyntaxNotifications(receiverCreator);

    public void RegisterForSyntaxNotifications(SyntaxContextReceiverCreator receiverCreator) => GeneratorInitializationContext.RegisterForSyntaxNotifications(receiverCreator);
}
