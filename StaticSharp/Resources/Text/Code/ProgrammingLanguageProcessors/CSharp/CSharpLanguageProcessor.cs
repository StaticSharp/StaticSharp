using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace StaticSharp.Gears {

    public class CSharpLanguageProcessor : ProgrammingLanguageProcessor {
        /*static CSharpLanguageProcessor() {
            AddProgrammingLanguageProcessor(new CSharpLanguageProcessor());
        }*/

        public override Inlines Highlight(string code, string programmingLanguageName, bool dark = false) {

            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            return base.Highlight(code, programmingLanguageName, dark);
        }


        protected override int Suitability(string name) {
            var names = new string[] { "cs", "csharp", "c#" };
            return names.Contains(name.ToLower()) ? 1 : 0;
        }
    }

}