using System.Collections.Generic;
using System.Linq;

namespace CsmlWeb {
    public struct Context { 
        public IStorage Storage { get; init; }

        public IUrls Urls { get; init; }

        public IIncludes Includes { get; init; }

        public IEnumerable<object> Parents;

   

        public Context(IStorage storage, IUrls urls) {
            Storage = storage;
            Urls = urls;
            Includes = new Includes();
            Parents = Enumerable.Empty<object>();
        }
    }
}