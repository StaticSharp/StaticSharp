using CsmlWeb;
using CsmlWeb.Html;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebsite.Content {

    class Logo : IInline {
        public static Color AntilatencyColor => Color.FromArgb(0xacc435);
        private readonly Color _primaryColor;
        private readonly Color _secondaryColor;
        private readonly dynamic _node;
        public Logo(Color primaryColor, Color secondaryColor, dynamic node) {
            _primaryColor = primaryColor;
            _secondaryColor = secondaryColor;
            _node = node;
        }

        public async Task<INode> GenerateInlineHtmlAsync(Context context) {
            context.Includes.Require(new Font(new RelativePath("..\\Fonts\\antilatency"), FontWeight.Regular, false));
            context.Includes.Require(new Style(new RelativePath("Logo.scss")));
            var uri = context.Urls.ObjectToUri(_node.Representative);
            return new Tag("a", new {
                Class = "AntilatencyLogo",
                style = $"font-family: 'antilatency' !important;",
                href = uri
            }) {
                new Tag("span", new { style = $"color: #{_primaryColor.ToRgbString()}"} ) { "a" },
                new Tag("span", new { style = $"color: #{_secondaryColor.ToRgbString()}"} ) { "l" },
            };
        }

        public void WriteHtml(StringBuilder builder) {
            throw new NotImplementedException();
        }
    }

    public static class LogoStatic {
        public static string ToRgbString(this Color color) => color.ToRgb().ToString("X6");

        public static uint ToRgb(this Color color) => (uint)color.ToArgb() & 0xFFFFFF;
    }

}
