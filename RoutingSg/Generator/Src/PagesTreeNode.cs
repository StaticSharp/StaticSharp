using Microsoft.CodeAnalysis;
using RoutingSg.Src.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RoutingSg.Src {
    public class PagesTreeNode { // TODO: review: replace with record? / replace Symbol with set of required names?
        public PagesTreeNode(INamespaceOrTypeSymbol symbol) {
            Symbol = symbol;
            IsNamespace = symbol is INamespaceSymbol; // TODO: review - init, getter, cacheing
            IsPage = StaticSharpConventions.IsPage(symbol);
            IsRepresentative = IsPage && StaticSharpConventions.IsPageRepresentative(symbol);
        }

        public INamespaceOrTypeSymbol Symbol { get; }

        public PagesTreeNode Parent { get; set; }

        public List<PagesTreeNode> Children { get; set; } = new List<PagesTreeNode>();


        // Helpers / cached

        public bool IsPage { get; }

        public bool IsRepresentative { get; }

        public bool IsNamespace { get; }

        // TODO: recursive calculation is suboptimal here, because tree is read by writers from root to leafs
        public bool ContainsRepresentatives {
            get {
                var containsRepresentatives = Children.Any(c => c.IsRepresentative || c.ContainsRepresentatives);

                // TODO: review edge case:
                if (containsRepresentatives && !IsNamespace && !((INamedTypeSymbol)Symbol).IsPartial()) {
                    // Error! For non-partial class, containing pages, it will be impossible to generated partail for child pages
                    return false; // temporary workaround
                }

                return containsRepresentatives;
            }
        }
    }
}
