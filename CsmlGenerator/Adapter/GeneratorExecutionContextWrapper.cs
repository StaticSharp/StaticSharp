using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Threading;


class GeneratorExecutionContextWrapper : IGeneratorExecutionContext {
    public GeneratorExecutionContext GeneratorExecutionContext { get; private set; }

    public GeneratorExecutionContextWrapper(GeneratorExecutionContext generatorExecutionContext) {
        GeneratorExecutionContext = generatorExecutionContext;
    }

    public Compilation Compilation => GeneratorExecutionContext.Compilation;

    public ParseOptions ParseOptions => GeneratorExecutionContext.ParseOptions;

    public ImmutableArray<AdditionalText> AdditionalFiles => GeneratorExecutionContext.AdditionalFiles;

    public AnalyzerConfigOptionsProvider AnalyzerConfigOptions => GeneratorExecutionContext.AnalyzerConfigOptions;

    public ISyntaxReceiver SyntaxReceiver => GeneratorExecutionContext.SyntaxReceiver;

    public ISyntaxContextReceiver SyntaxContextReceiver => GeneratorExecutionContext.SyntaxContextReceiver;

    public CancellationToken CancellationToken => GeneratorExecutionContext.CancellationToken;

    public void AddSource(string hintName, string source) => GeneratorExecutionContext.AddSource(hintName, source);    

    public void AddSource(string hintName, SourceText sourceText) => GeneratorExecutionContext.AddSource(hintName, sourceText);    

    public void ReportDiagnostic(Diagnostic diagnostic) => GeneratorExecutionContext.ReportDiagnostic(diagnostic);
}
