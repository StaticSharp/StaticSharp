using System.Threading.Tasks;

namespace CsmlWeb {

    public interface IInclude : IKey {

        Task<string> GenerateAsync(IStorage storage);
    }
}