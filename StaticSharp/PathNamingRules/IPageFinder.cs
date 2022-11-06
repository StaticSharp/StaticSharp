using StaticSharp.Gears;

namespace StaticSharp {
    public interface IPageFinder {
        IPageGenerator? FindPage(string requestPath);
    }




}
