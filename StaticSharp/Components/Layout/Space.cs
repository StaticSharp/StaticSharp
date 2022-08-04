
using StaticSharpWeb.Html;
using System.Threading.Tasks;

namespace StaticSharpWeb {
    public class Space : Component, IInline, IContainerConstraintsNone {
        public float Size { get; }
        public Space(float size = 1.0f) {
            Size = size;
        }
        public override Task<Tag> GenerateHtmlAsync(Context context) {
            return Task.FromResult(new Tag("space", this));
        }
    }

}
