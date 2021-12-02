using System.Collections.Generic;
using System.Linq;

namespace StaticSharpGenerator {    
    partial class Builder {

        public IEnumerable<TypeInfo> FindRoots() {
            var result = NamespaceInfo.GetAllNamespaces()
                .SelectMany(x => x.Types.Where(t => t.Key == ProtoNode))                
                .Select(x => x.Value);
            return result;
        }
    }
}