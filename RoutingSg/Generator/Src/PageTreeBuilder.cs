using Microsoft.CodeAnalysis;
using System.Linq;
using System.Collections.Generic;
using RoutingSg.Src;
using RoutingSg.Src.Helpers;

namespace RoutingSg {

    class PageTreeBuilder {

        public StateParameter[] State { get; }

        protected Log _log { get; } // TODO: clarify how it works

        protected Compilation _compilation { get; }

        protected string _rootNamespaceName { get; }

        protected PagesTreeNode _representativesTree { get; set; }

        protected Dictionary<string, PagesTreeNode> _representativesTreeDictionary { get; } = new Dictionary<string, PagesTreeNode>(); // Key: fully qualified name

        public PageTreeBuilder(GeneratorExecutionContext executionContext) {
            _log = new Log(executionContext);
            _compilation = executionContext.Compilation;

            // TODO: validate Root namespace present and contains Language enum, report error

            var languageEnum = StaticSharpConventions.GetLanguageEnum(_compilation);

            State = new StateParameter[] {
                new StateParameter(
                    languageEnum.Name.ToLower()/* "language"*/,
                    languageEnum, //Compilation.GetSymbolsWithName(Kw.Language).FirstOrDefault() as ITypeSymbol, // TODO: more preciese filter
                    languageEnum.Name //Kw.Language
                )
            };

            _rootNamespaceName = StaticSharpConventions.GetRootNamespaceFullName(_compilation);
        }

        public PagesTreeNode Build() {

            var allSymbols = _compilation.GetSymbolsWithName(_ => true);
            var typeSymbols = allSymbols.OfType<INamedTypeSymbol>();
            var allPages = typeSymbols.Where(StaticSharpConventions.IsPage);

            foreach (var pageSymbol in allPages) {
                ProcessSymbol(pageSymbol, null);
            }

            return _representativesTree;
        }

        // Traverse tree from leaf to root
        protected void ProcessSymbol(INamespaceOrTypeSymbol symbol, PagesTreeNode addToChildrenNode) {
            var fullyQualifiedName = symbol?.GetFullyQualifiedNameNoGlobal();
            if (fullyQualifiedName == null || !fullyQualifiedName.StartsWith(_rootNamespaceName))
                return; // branch not from our root, or gone beyound root

            var nodeProcessed = _representativesTreeDictionary.TryGetValue(fullyQualifiedName, out var pageTreeNode);
            if (!nodeProcessed) {
                pageTreeNode = new PagesTreeNode(symbol);
                _representativesTreeDictionary[fullyQualifiedName] = pageTreeNode;

                if (_representativesTree == null && fullyQualifiedName == _rootNamespaceName) {
                    _representativesTree = pageTreeNode;
                }
            }

            if (addToChildrenNode != null) {
                pageTreeNode.Children.Add(addToChildrenNode);
                addToChildrenNode.Parent = pageTreeNode;
            }

            if (!nodeProcessed) {
                // container is not namespace and not type (impossible?), or traverse reached global namespace
                if (!(symbol.ContainingSymbol is INamespaceOrTypeSymbol parentSymbol)) return;
                ProcessSymbol(parentSymbol, pageTreeNode);
            }
        }
    }


}