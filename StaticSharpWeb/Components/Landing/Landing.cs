using StaticSharpEngine;
using StaticSharpWeb.Html;
using StaticSharpWeb.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public enum Position {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Top,
        Bottom
    }
    public class Landing : IBlock
    {
        private Image _image { get; set; }
        private string _titleText{ get; set; }
        private string _text { get; set; }
        //private int _position { get; set; }
        private Position _position { get; set; }

        public Landing(Image image, string titleText, string text, Position position) {
            _image = image;
            _titleText = titleText;
            _text = text;
            _position = position;
        }

        public async Task<Html.INode> GenerateBlockHtmlAsync(Context context)
        {
            var tag = new Tag("div", new { Class = "LandingContainer", id = "LandingContainer" });
            var imageTag = new Tag("div", new { Class = "InnerImageContainer", id = "InnerImageContainer" }) { 
                await _image.GenerateBlockHtmlAsync(context)
            };
            var titleText = new Tag("h2") { _titleText };
            var text = new Tag("div") { _text };
            var textTag = new Tag("div", new { Class = "TextContainer", id = "TextContainer"});
            textTag.Add(titleText);
            textTag.Add(text);
            tag.Add(imageTag);
            tag.Add(textTag);
            context.Includes.Require(new Style(new RelativePath("Landing.scss")));
            tag.Add(new JSCall(new RelativePath(_position.ToString() + "Landing.js")).Generate(context));
            return tag;
        }
    }

    public static class LandingStatic {
        public static void Add(this IVerifiedBlockReceiver collection, Landing item) {
            collection.AddBlock(item);
        }
    }
}