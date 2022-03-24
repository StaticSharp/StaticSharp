using StaticSharpEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharpWeb {


    public struct Context { 
        public IStorage Storage { get; init; }

        public INodeToUrl NodeToUrlConverter { get; init; }
        public Uri? NodeToUrl(INode node) { 
            return NodeToUrlConverter.NodeToUrl(BaseUrl, node);
        }
        public Uri BaseUrl { get; init; }

        public IIncludes Includes { get; init; }

        public IEnumerable<object> Parents;

        public Theme Theme;


        public Font Font = new Font(DefaultFont.Arial);

        public float FontSize = 16;


        public Context(IStorage storage, Uri baseUrl, Theme theme, INodeToUrl nodeToUrlConverter) {
            Storage = storage;
            //Urls = urls;
            BaseUrl = baseUrl;
            Theme = theme;
            Includes = new Includes();
            Parents = Enumerable.Empty<object>();
            NodeToUrlConverter = nodeToUrlConverter;
        }
    }
}