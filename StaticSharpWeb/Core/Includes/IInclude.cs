using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface IInclude : IKey {

        Task<string> GenerateAsync(IStorage storage);
    }
}