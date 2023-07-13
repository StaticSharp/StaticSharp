using StaticSharpDemo.Root;
using StaticSharp.Gears;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.CompilerServices;

namespace StaticSharpDemo {

    internal class Program: StaticSharp.Program {

        private static Task Main() => RunEntryPointFromEnvironmentVariable<Program>();


        public static async Task Server() {
            Cache.Directory = Configuration.GetVariable("cachePath", MakeAbsolutePath(".cache"));

            await new Server(
                new DefaultMultilanguagePageFinder<Language>((language) => new αRoot(language)),
                new DefaultMultilanguageNodeToPath<Language>()

                ).RunAsync();
        }


        public static async Task Generator() {

            var targetHostName = Configuration.GetVariable("targetHostName");
            Cache.Directory = Configuration.GetVariable("cachePath", MakeAbsolutePath(".cache"));
            
            var outputDirectory = Configuration.GetVariable("outputDirectory");
            if (!Path.IsPathFullyQualified(outputDirectory)) {
                outputDirectory = Path.GetFullPath(Path.Combine(GetThisFileDirectory(), outputDirectory));
            }

            var generator = new MultilanguageStaticGenerator<Language>(
                new DefaultMultilanguageNodeToPath<Language>(),
                new AbsoluteUrl("https", targetHostName),
                FilePath.FromOsPath(outputDirectory)
                );

            await generator.GenerateAsync(new αRoot(default));
        }

    }

}
