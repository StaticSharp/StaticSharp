using Microsoft.CodeAnalysis;
//using Scopes;
//using Scopes.C;
using System.Linq;

namespace StaticSharpGenerator {

    partial class Builder {
        public Log Log { get; }
        public Compilation Compilation { get; }
        public BlockWriter ClassTree { get; }
        public BlockWriter Partials { get; }

        


        public NamespaceInfo NamespaceInfo { get; }

        //List<TypeInfo> Representatives { get; set; }
        private TypeInfo Root;

        public string RootNamespaceName;
        private StateParameter[] State;

        public Builder(Compilation compilation, IGeneratorExecutionContext executionContext) {
            Log = new Log(executionContext);
            Compilation = compilation;

            ClassTree = new BlockWriter();
            Partials = new BlockWriter();

            NamespaceInfo = Collect();
            var roots = FindRoots();

            ClassTree.AddLine("using System.Collections.Generic;");
            ClassTree.AddLine("using System.Linq;");
            ClassTree.AddLine("using System;");
            ClassTree.AddLine("#pragma warning disable IDE1006 // Naming Styles");

            foreach (var root in roots) {
                BuildRoot(root);
            }
        }

        private void BuildRoot(TypeInfo root) {
            //BlockWriter classTree = classTreeFile;
            var rootSymbol = Compilation.GetTypeByMetadataName(root.FullyQualifiedName);
            RootNamespaceName = rootSymbol.ContainingNamespace.GetFullyQualifiedName();

            var constructors = rootSymbol.Constructors.Where(x => !x.IsImplicitlyDeclared).ToArray(); //.Where(x => x.Parameters.Count() == 1);
            if (constructors.Length != 1) {
                throw new System.IndexOutOfRangeException("Root symbol must have at least one constructor");
            }

            State = constructors[0].Parameters.Select(
                    x => new StateParameter(x.Name, x.Type, x.Type.GetFullyQualifiedName())
                ).ToArray();

            /*var constructor = constructors.First();
            var parameter = constructor.Parameters[0];

            var stateName = parameter.Name;
            var stateType = parameter.Type.GetFullyQualifiedName();*/

            var namespaceInfo = root.Parent as NamespaceInfo;

            var classPlace = ClassTree;
            if (namespaceInfo.Name != null) {
                classPlace = ClassTree.AddLine(new NamespaceWriter(namespaceInfo.FullName)).Content;
            }

            /*classPlace.AddLine(new HeaderBracesWriter($"interface {IRepresentative}"))
                .Content.AddLine($"{Node} {NodePropertyName} {{ get; }}");*/

            //var classBody = classPlace.AddLine(new HeaderBracesWriter($"/*Generated*/ partial {root.Keyword} {CsmlRoot}{State.ToRecordParametersDeclaration()}: {Node}{State.ToBaseCall()}, {INode}")).Content;

            WriteClassTree(AlphaRoot, namespaceInfo, classPlace, null, Enumerable.Empty<string>());

            /*classBody.AddLine($"{INode} {INode}.Parent => null;");

            WriteChildren(namespaceInfo, classBody);

            foreach (var n in namespaceInfo.Namespaces) {
                WriteClassTree(n.Value, classBody, CsmlRoot);
            }*/

            /*classPlace = Partials;
            if (namespaceInfo.Name != null) {
                classPlace = Partials.AddLine(new NamespaceWriter(namespaceInfo.FullName)).Content;
            }*/

            WritePartials(namespaceInfo, Partials, Enumerable.Empty<string>());




        }
    }
}