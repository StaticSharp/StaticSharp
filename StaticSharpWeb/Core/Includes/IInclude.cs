using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface IInclude : IKey {

        Task<string> GenerateAsync(IStorage storage);
        //string GenerateSuperStyle(string file);
    }
}