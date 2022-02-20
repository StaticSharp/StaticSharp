using StaticSharpWeb;
using StaticSharpWeb.Html;

using System.Drawing;

using System.Threading.Tasks;

namespace StaticSharpDemo.Content {

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

        public async Task<StaticSharpWeb.Html.INode> GenerateInlineHtmlAsync(Context context) {
            context.Includes.Require(new Font(AbsolutePath("..\\Fonts\\antilatency"), FontWeight.Regular, false));
            context.Includes.Require(new Style(AbsolutePath("Logo.scss")));
            var uri = context.Urls.ProtoNodeToUri(_node);
            return new Tag("a", new {
                Class = "AntilatencyLogo",
                style = $"font-family: 'antilatency' !important;",
                href = uri
            }) {
                new Tag("span", new { style = $"color: #{_primaryColor.ToRgbHexString()}"} ) { "a" },
                new Tag("span", new { style = $"color: #{_secondaryColor.ToRgbHexString()}"} ) { "l" },
            };
        }

    }


}
