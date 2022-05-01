using StaticSharpEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp.Gears {


    public struct Context { 
        //public Assets Storage { get; init; }

        public INodeToUrl NodeToUrlConverter { get; init; }
        public Uri? NodeToUrl(INode node) { 
            return NodeToUrlConverter.NodeToUrl(BaseUrl, node);
        }
        public Uri BaseUrl { get; init; }

        public IIncludes Includes { get; init; }


        private FontFamily fontFamily = null!;
        public FontFamily FontFamily {
            get { return fontFamily; }
            set {
                cacheableFontFamily = null;
                fontFamily = value;
            }
        }

        private CacheableFontFamily cacheableFontFamily = null!;
        public async Task<CacheableFontFamily> GetCacheableFontFamily() {
            if (cacheableFontFamily == null) {
                cacheableFontFamily = await FontFamily.CreateOrGetCached();
            }
            return cacheableFontFamily;
        }

        private FontStyle fontStyle = new();
        public FontStyle FontStyle {
            get { return fontStyle; }
            set {
                if (fontStyle != value) {
                    Font = new Font();
                    fontStyle = value;
                }
            }
        }

        private Font? font = null;
        public Font Font {
            get { return font; }
            set {
                if (font != value) {
                    cacheableFont = null;
                    font = value;
                }
            }
        }


        private CacheableFont? cacheableFont = null;
        public async Task<CacheableFont> GetCacheableFont() {
            if (cacheableFont == null) {
                cacheableFont = await font.CreateOrGetCached();
            }
            return cacheableFont;
        }



        //public CacheableFont Font = null!;
        public float FontSize = 16;
        public ITextMeasurer TextMeasurer = null!;

        public Context(Uri baseUrl, INodeToUrl nodeToUrlConverter) {
            //Urls = urls;
            BaseUrl = baseUrl;
            Includes = new Includes();
            //Parents = Enumerable.Empty<object>();
            NodeToUrlConverter = nodeToUrlConverter;
        }
    }
}