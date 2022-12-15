using StaticSharp.Html;

namespace StaticSharp {
    public interface IBlock {
        public Tag GenerateHtml(Context context, Role? role);
    }
}