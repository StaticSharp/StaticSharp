using System.Threading.Tasks;

namespace StaticSharp.Gears {

    public interface IInclude : IKeyProvider {

        Task<string> GenerateIncludeAsync();
        //string GenerateSuperStyle(string file);
    }
}