using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsmlWeb.Html;

namespace CsmlWeb {
    public class Reference : IBlock
    {
        private string _href { get; set; }
        private string _text { get; set; }
        private string _tooltip { get; set; }
        private Image _image { get; set; }
        private string _hrefText { get; set; }

        public Reference(string href, string hrefText, string text = null, string tooltip = null) 
            => (_href, _hrefText, _text, _tooltip) = (href, hrefText, text, tooltip);

        public Reference(string href, string hrefText, Image image, string text = null, string tooltip = null) : this(href, hrefText, text, tooltip) 
            => _image = image;

        //private void SetHref(string href) => _href = href;
        public async Task<INode> GenerateBlockHtmlAsync(Context context)
        {
            INode node = null;
            if (_image != null) {
                node = await _image.GenerateBlockHtmlAsync(context);
                _hrefText = null;
            }

            if (_tooltip == null) {
                _tooltip = _href;
            }

            var componentName = nameof(Reference);
            var tag = new Tag("div", new { Class = "Text"});
            tag.Add(_text);
            tag.Add(new Tag("a", new { Class = "Reference", Href = _href, Title = _tooltip}) {
                _hrefText,
                node
            });
            //CsmlWeb.Components.Video video;

            //await _image.GenerateBlockHtmlAsync(context);

            

            // tag.Add(new Tag("a", new {Class = "Reference"}));
            // tag.Attributes.Add("href", _href);
            // tag.Add(_text);
            // tag.Attributes.Add("Title", _tooltip);
            
            tag.Add(new JSCall(new RelativePath(componentName + ".js")).Generate(context));
            //tag.Add(_href);
            //tag.Add(_tooltip);
            return tag;
        }
    }

    public static class ReferenceStatic {
        public static void Add(this IVerifiedBlockReceiver collection, Reference item) {
            collection.AddBlock(item);
        }
    }  
}