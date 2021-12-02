using Microsoft.CodeAnalysis;
using System;
using System.Threading;

class GeneratorInitializationContextDebug : IGeneratorInitializationContext {

    public SyntaxReceiverCreator syntaxReceiverCreator { get; private set; }
    public SyntaxContextReceiverCreator syntaxContextReceiverCreator { get; private set; }


    public GeneratorInitializationContextDebug() {
    }



    public CancellationToken CancellationToken => throw new NotImplementedException();

    public void RegisterForSyntaxNotifications(SyntaxReceiverCreator receiverCreator) {
        syntaxReceiverCreator = receiverCreator;
    }

    public void RegisterForSyntaxNotifications(SyntaxContextReceiverCreator receiverCreator) {
        syntaxContextReceiverCreator = receiverCreator;
    }

    public void RegisterForPostInitialization(Action<GeneratorPostInitializationContext> callback) {
        throw new NotImplementedException();
    }
}