using System.Collections.Immutable;
using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace Exo.RoslynSourceGeneratorDebuggable {
    class GeneratorExecutionContextDebug : IGeneratorExecutionContext {

        public Dictionary<string, string> Results = new Dictionary<string, string>();

        /*private Compilation compilation;
        public Compilation Compilation {
            get {
                if (compilation == null)
                    compilation = CSharpCompilation.Create("", SyntaxTrees);
                return compilation;
            }
        }*/

        public Compilation Compilation { get; set; }

        public IEnumerable<SyntaxTree> SyntaxTrees => Compilation.SyntaxTrees;

        public GeneratorExecutionContextDebug(GeneratorInitializationContextDebug generatorInitializationContext, Compilation compilation) {
            //SyntaxTrees = syntaxTrees;
            Compilation = compilation;
            SyntaxReceiver = generatorInitializationContext.syntaxReceiverCreator?.Invoke();
            SyntaxContextReceiver = generatorInitializationContext.syntaxContextReceiverCreator?.Invoke();

            //ProcessSyntaxTrees();
        }

        /*private void ProcessSyntaxTrees() {
            if (SyntaxReceiver != null) {
                var generatorSyntaxWalker = new GeneratorSyntaxWalker(SyntaxReceiver);
                foreach (var t in SyntaxTrees)
                    generatorSyntaxWalker.Visit(t.GetRoot());
            }
        }*/


        public ISyntaxReceiver SyntaxReceiver { get; private set; }

        public ISyntaxContextReceiver SyntaxContextReceiver { get; private set; }




        public ParseOptions ParseOptions => throw new System.NotImplementedException();

        public ImmutableArray<AdditionalText> AdditionalFiles => throw new System.NotImplementedException();

        public AnalyzerConfigOptionsProvider AnalyzerConfigOptions => throw new System.NotImplementedException();

        public CancellationToken CancellationToken => throw new System.NotImplementedException();

        public void AddSource(string hintName, string source) {
            Results.Add(hintName, source);
        }

        public void AddSource(string hintName, SourceText sourceText) {
            AddSource(hintName, sourceText.ToString());
        }

        public void ReportDiagnostic(Diagnostic diagnostic) {

        }
    }
}
