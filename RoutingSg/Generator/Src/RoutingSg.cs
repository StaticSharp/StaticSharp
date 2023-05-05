using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;



public abstract class IndentWriter {
    public abstract void Build(StringBuilder builder, int indent);

    public override string ToString() {
        var builder = new StringBuilder();
        Build(builder, 0);
        return builder.ToString();
    }
}

public class BlockWriter : IndentWriter {

    List<object> lines = new List<object>();


    public BlockWriter() {
    }

    public bool IsEmpty => lines.Count == 0;

    public T AddLine<T>(T content) where T : class {
        lines.Add(content);
        return content;
    }




    public override void Build(StringBuilder builder, int indent) {
        foreach(var i in lines) {
            if(i is IndentWriter indentWriter) {
                indentWriter.Build(builder, indent);
            } else {
                builder.Append('\t', indent).AppendLine(i.ToString());
            }

        }
    }
}

public class HeaderBracesWriter : IndentWriter {
    //braces
    public HeaderBracesWriter(string prefix) {
        Header = prefix;

    }
    public string Header { get; set; }


    public int IndentIncrement { get; set; } = 1;

    private BlockWriter content = null;
    public BlockWriter Content {
        get {
            if(content == null) {
                content = new BlockWriter();
            }
            return content;
        }
        set {
            content = value;
        }
    }

    

    public override void Build(StringBuilder builder, int indent) {
        builder.Append('\t', indent).Append(Header).AppendLine(" {");
        content?.Build(builder, indent + IndentIncrement);
        builder.Append('\t', indent).AppendLine("}");
    }
}

public class NamespaceWriter : HeaderBracesWriter {
    public NamespaceWriter(string name) : base($"namespace {name}") { }
}





namespace RoutingSg {




    [Generator]
    public class RoutingSg : ISourceGenerator
    {

        public void Initialize(GeneratorInitializationContext context) {
            //Debugger.Launch();
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
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


            var builder = new Builder(context.Compilation, context);


            context.AddSource("classTree.cs", builder.ClassTree.ToString());
            context.AddSource("partials.cs", builder.Partials.ToString());

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"Generator.Execute time: {elapsedMs} ms");

        }
    }
}