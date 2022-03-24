using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface IInclude : IKey {

        Task<string> GenerateIncludeAsync(IStorage storage);
        //string GenerateSuperStyle(string file);
    }
}