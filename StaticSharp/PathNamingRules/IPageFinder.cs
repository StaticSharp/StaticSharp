using StaticSharp.Gears;
using StaticSharp.Tree;

namespace StaticSharp {
    public interface IPageFinder {
        Page? FindPage(string requestPath);

    }
}
