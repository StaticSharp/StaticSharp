using StaticSharpEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharp.Gears {


    public struct Context { 
        //public Assets Storage { get; init; }

        public INodeToUrl NodeToUrlConverter { get; init; }
        public Uri? NodeToUrl(INode node) { 
            return NodeToUrlConverter.NodeToUrl(BaseUrl, node);
        }
        public Uri BaseUrl { get; init; }

        public IIncludes Includes { get; init; }

        //public IEnumerable<object> Parents;


        public Font Font = new Font(DefaultFont.Arial);

        public float FontSize = 16;


        public Context(Uri baseUrl, INodeToUrl nodeToUrlConverter) {
            //Urls = urls;
            BaseUrl = baseUrl;
            Includes = new Includes();
            //Parents = Enumerable.Empty<object>();
            NodeToUrlConverter = nodeToUrlConverter;
        }
    }
}