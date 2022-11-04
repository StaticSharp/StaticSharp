using System;

namespace StaticSharp.Gears {
    public interface INodeToUrl {
        //public Uri BaseUrl { get; }

        public Uri? NodeToUrl(Uri baseUrl, Tree.INode node);
    }
}