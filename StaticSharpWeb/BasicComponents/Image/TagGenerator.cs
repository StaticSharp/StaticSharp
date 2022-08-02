using StaticSharp.Gears;
using StaticSharp.Html;
using System.Threading.Tasks;

namespace StaticSharp {
    public abstract record TagGenerator: Genome {
        public abstract Task<Tag> Generate(string id);
    }

}

