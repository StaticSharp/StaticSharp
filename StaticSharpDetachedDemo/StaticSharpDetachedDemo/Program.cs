using StaticSharp.Gears;
using StaticSharp;
using System.Threading.Tasks;
using StaticSharpDetachedDemo.Root;

namespace StaticSharpDetachedDemo
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Cache.Directory = Static.MakeAbsolutePath(".cache");

            await new StaticSharp.Server(
                new DefaultMultilanguagePageFinder<Language>((language) => new αRoot(language)),
                new DefaultMultilanguageNodeToPath<Language>()

                ).RunAsync();
        }
    }
}