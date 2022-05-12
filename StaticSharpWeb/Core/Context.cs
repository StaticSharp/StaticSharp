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
                if (fontFamily != value) {
                    fontFamily = value;
                    Font = new Font(fontFamily, fontStyle);
                    //cacheableFontFamily = null;
                }
            }
        }

        private FontStyle fontStyle = new();
        public FontStyle FontStyle {
            get { return fontStyle; }
            set {
                if (value == null) {
                    value = new();
                }
                if (fontStyle != value) {
                    fontStyle = value;
                    if (fontFamily != null) {
                        Font = new Font(fontFamily, fontStyle);
                    } else {
                        Font = null;
                    }                    
                }
            }
        }

        private Font? font = null;
        public Font? Font {
            get { return font; }
            private set {
                if (font != value) {
                    font = value;
                    cacheableFont = null;
                    //textMeasurer = null;
                }
            }
        }

        private CacheableFont? cacheableFont = null;
        public async ValueTask<CacheableFont> GetCacheableFont() {
            if (cacheableFont == null) {
                if (font == null) {
                    throw new InvalidOperationException("FontFamily is not set.");
                }
                cacheableFont = await font.CreateOrGetCached();
            }
            return cacheableFont;
        }

        /*public ITextMeasurer? textMeasurer = null;
        public async ValueTask<ITextMeasurer> GetTextMeasurer() {
            if (textMeasurer == null) {
                textMeasurer = (await GetCacheableFont()).CreateTextMeasurer(fontSize);
            }
            return textMeasurer;
        }*/


        //public CacheableFont Font = null!;
        /*public float fontSize = 16;
        public float FontSize {
            get { return fontSize; }
            set {
                if (fontSize != value) {
                    fontSize = value;
                    textMeasurer = null;
                }
            }
        }*/

        public Context(Uri baseUrl, INodeToUrl nodeToUrlConverter) {
            //Urls = urls;
            BaseUrl = baseUrl;
            Includes = new Includes();
            //Parents = Enumerable.Empty<object>();
            NodeToUrlConverter = nodeToUrlConverter;
        }
    }
}