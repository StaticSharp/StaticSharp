using Exo.RoslynSourceGeneratorDebuggable;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpGenerator {
    class Log {
        public IGeneratorExecutionContext ExecutionContext { get; }
        //public DiagnosticSeverity Severity { get; }

        public Log(IGeneratorExecutionContext executionContext) {
            ExecutionContext = executionContext;
        }

        /*public static Log Error() {
            return new Log(DiagnosticSeverity.Error);            
        }


        public static Log Warning {
            get {
                return new Log(DiagnosticSeverity.Warning);
            }
        }
        public static Log Info {
            get {
                return new Log(DiagnosticSeverity.Info);
            }
        }*/

        private void ReportError(string message, Location location, [CallerFilePath] string filePath = null, [CallerLineNumber] int line = 0) {
            var id = Path.GetFileName(filePath) + line.ToString("D4");

            var descriptor = new DiagnosticDescriptor(id, "title", message, "category", DiagnosticSeverity.Error, true);
            ExecutionContext.ReportDiagnostic(Diagnostic.Create(descriptor, location));

        }

#line 1 "StaticSharp"
        public void AbstractRepresentative(Location location) => ReportError("Abstract Representative",location);


        //public static void AbstractRepresentative()


    }




    /*class DiagnosticMessage {


#line 1 "Special"


#line default
        static DiagnosticMessage MakeReport() {
        
        }
    }*/
}
