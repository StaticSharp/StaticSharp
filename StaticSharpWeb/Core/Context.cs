using System.Collections.Generic;
using System.Linq;

namespace StaticSharpWeb {


    public struct Context { 
        public IStorage Storage { get; init; }

        public IUrls Urls { get; init; }

        public IIncludes Includes { get; init; }

        public IEnumerable<object> Parents;

        public Theme Theme;

        public Context(IStorage storage, IUrls urls, Theme theme) {
            Storage = storage;
            Urls = urls;
            Theme = theme;
            Includes = new Includes();
            Parents = Enumerable.Empty<object>();
        }
    }
}