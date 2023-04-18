using System.Reflection;
using static Javascriptifier.ExpressionScriptifier;

namespace StaticSharp {



    namespace Gears {
        public record FileOfAssemblyResourceGenome(Assembly Assembly, string Path) : Genome<Genome<IAsset>> {
            protected override void Create(out Genome<IAsset> value, out Func<bool> verify) {
                verify = () => true;
                if (File.Exists(Path)) {
                    
                    value = new FileGenome(Path);
                } else {
                    var relativeFilePath = AssemblyResourcesUtils.GetFilePathRelativeToProject(Assembly, Path);
                    var relativeResourcePath = AssemblyResourcesUtils.GetResourcePath(relativeFilePath);

                    value = new AssemblyResourceGenome(Assembly, relativeResourcePath);
                }
            }
        }
    }
}

