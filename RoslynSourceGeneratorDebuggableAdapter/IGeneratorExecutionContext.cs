using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Threading;

namespace Exo.RoslynSourceGeneratorDebuggable {

    public interface IGeneratorExecutionContext {
        //
        // Summary:
        //     Get the current Microsoft.CodeAnalysis.GeneratorExecutionContext.Compilation
        //     at the time of execution.
        //
        // Remarks:
        //     This compilation contains only the user supplied code; other generated code is
        //     not available. As user code can depend on the results of generation, it is possible
        //     that this compilation will contain errors.
        Compilation Compilation { get; }
        //
        // Summary:
        //     Get the Microsoft.CodeAnalysis.GeneratorExecutionContext.ParseOptions that will
        //     be used to parse any added sources.
        ParseOptions ParseOptions { get; }
        //
        // Summary:
        //     A set of additional non-code text files that can be used by generators.
        ImmutableArray<AdditionalText> AdditionalFiles { get; }
        //
        // Summary:
        //     Allows access to options provided by an analyzer config
        AnalyzerConfigOptionsProvider AnalyzerConfigOptions { get; }
        //
        // Summary:
        //     If the generator registered an Microsoft.CodeAnalysis.ISyntaxReceiver during
        //     initialization, this will be the instance created for this generation pass.
        ISyntaxReceiver SyntaxReceiver { get; }
        //
        // Summary:
        //     If the generator registered an Microsoft.CodeAnalysis.ISyntaxContextReceiver
        //     during initialization, this will be the instance created for this generation
        //     pass.
        ISyntaxContextReceiver SyntaxContextReceiver { get; }
        //
        // Summary:
        //     A Microsoft.CodeAnalysis.GeneratorExecutionContext.CancellationToken that can
        //     be checked to see if the generation should be cancelled.
        CancellationToken CancellationToken { get; }

        //
        // Summary:
        //     Adds source code in the form of a System.String to the compilation.
        //
        // Parameters:
        //   hintName:
        //     An identifier that can be used to reference this source text, must be unique
        //     within this generator
        //
        //   source:
        //     The source code to add to the compilation
        void AddSource(string hintName, string source);
        //
        // Summary:
        //     Adds a Microsoft.CodeAnalysis.Text.SourceText to the compilation
        //
        // Parameters:
        //   hintName:
        //     An identifier that can be used to reference this source text, must be unique
        //     within this generator
        //
        //   sourceText:
        //     The Microsoft.CodeAnalysis.Text.SourceText to add to the compilation
        void AddSource(string hintName, SourceText sourceText);
        //
        // Summary:
        //     Adds a Microsoft.CodeAnalysis.Diagnostic to the users compilation
        //
        // Parameters:
        //   diagnostic:
        //     The diagnostic that should be added to the compilation
        //
        // Remarks:
        //     The severity of the diagnostic may cause the compilation to fail, depending on
        //     the Microsoft.CodeAnalysis.GeneratorExecutionContext.Compilation settings.
        void ReportDiagnostic(Diagnostic diagnostic);
    }

}