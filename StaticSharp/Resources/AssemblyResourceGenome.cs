using StaticSharp.Gears;
using System.Reflection;



namespace StaticSharp {

    public record AssemblyResourceGenome(Assembly Assembly, string Path) : Genome<IAsset> {

        protected override void Create(out IAsset value, out Func<bool> verify) {

            var resourcePath = Assembly.GetName().Name + "." + Path;
            using var stream = Assembly.GetManifestResourceStream(resourcePath);

            if (stream == null) {
                throw new FileNotFoundException(resourcePath);
            }


            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                var data = memoryStream.ToArray();
                value = new BinaryAsset(
                    data,
                    System.IO.Path.GetExtension(Path) ?? ""
                    );

                verify = () => true;
            }
        }
    }

 
    namespace Gears {
        public static partial class KeyCalculators {
            public static string GetKey(Assembly assembly) {
                return KeyUtils.Combine<Assembly>(assembly.FullName);
            }
        }        
    }
}

