
using StaticSharpWeb.Html;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public class Spacer : Component, IInline, IContainerConstraintsNone {
        public float Size { get; }
        public Spacer(float size = 1.0f) {
            Size = size;
        }
        public override Task<Tag> GenerateHtmlAsync(Context context) {
            return Task.FromResult(new Tag("div", new {
                Spacer = Size,
                style = new {
                    flexGrow = Size
                }
            }));
        }
    }

}
