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
    public class Landing : IBlock
    {
        private Image _image { get; set; }
        private Paragraph _paragraph { get; set; }
        private int _position { get; set; }

        public Landing(Image image, Paragraph paragraph, int position) {
            _image = image;
            _paragraph = paragraph;
            _position = position;
        }

        public async Task<Html.INode> GenerateBlockHtmlAsync(Context context)
        {
            var tag = new Tag("div", new { Class = "LandingContainer" });
            tag.Add(await _image.GenerateBlockHtmlAsync(context));
            tag.Add(await _paragraph.GenerateBlockHtmlAsync(context));
            context.Includes.Require(new Style(new RelativePath("Landing.scss")));
            tag.Add(new JSCall(new RelativePath("Landing.js")).Generate(context));
            return tag;
        }
    }

    public static class LandingStatic {
        public static void Add(this IVerifiedBlockReceiver collection, Landing item) {
            collection.AddBlock(item);
        }
    }
}