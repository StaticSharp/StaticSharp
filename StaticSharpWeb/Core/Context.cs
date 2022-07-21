using StaticSharpEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp.Gears {


    public struct Context { 

        public INodeToUrl NodeToUrlConverter { get; init; }
        public Uri? NodeToUrl(INode node) { 
            return NodeToUrlConverter.NodeToUrl(BaseUrl, node);
        }
        public Uri BaseUrl { get; init; }

        public Uri BaseAssetsUrl => new Uri(BaseUrl, "Assets/");

        public IIncludes Includes { get; init; }

        public Assets Assets { get; init; }


        public FontFamily[] FontFamilies { get; set; } = null!;
        public FontStyle FontStyle { get; set; } = new();

        public Uri AddAsset(IAsset asset) {
            Assets.Add(asset);
            return new Uri(BaseAssetsUrl, asset.FilePath);
        }

        public Context(Assets assets, Uri baseUrl, INodeToUrl nodeToUrlConverter) {
            Assets = assets;
            //Urls = urls;
            BaseUrl = baseUrl;
            Includes = new Includes();
            //Parents = Enumerable.Empty<object>();
            NodeToUrlConverter = nodeToUrlConverter;
        }
    }
}