using StaticSharpDemo.Root;
using StaticSharp.Gears;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace StaticSharpDemo {





    internal class Program {

        private static async Task Main(string[] args) {



            //var ls = new LambdaScriptifier(() => Color2.Black + Color2.White);
            //var code = ls.Eval();


            var entryPointName = Environment.GetEnvironmentVariable("ENTRY_POINT");
            if (entryPointName == null) {
                Console.WriteLine("EnvironmentVariable 'ENTRY_POINT' not found.");
                return;
            }

            var entryPoint = typeof(Program).GetMethod(entryPointName,
                System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Static
                | System.Reflection.BindingFlags.FlattenHierarchy);

            if (entryPoint == null) {
                Console.WriteLine($"entryPoint {entryPointName} not found.");
                return;
            }

            if (entryPoint.ReturnType == typeof(Task)) {
                var task = entryPoint.Invoke(null, null) as Task;
                if (task != null) {
                    await task;
                }
            } else {
                entryPoint.Invoke(null, null);
            }
        }

        public static async Task Server() {
            Cache.RootDirectory = MakeAbsolutePath(".cache");

            await new StaticSharp.Server(
                new DefaultMultilanguagePageFinder<Language>((language) => new αRoot(language)),
                new DefaultMultilanguageNodeToPath<Language>()

                ).RunAsync();
        }

        public static async Task Generator() {
            Cache.RootDirectory = MakeAbsolutePath(".cache");

            var projectPath = ProjectDirectory.Path;
            var baseDirectory = Path.GetFullPath(Path.Combine(projectPath, "../../StaticSharp.github.io"));

            var generator = new MultilanguageStaticGenerator<Language>(
                new DefaultMultilanguageNodeToPath<Language>(),
                new AbsoluteUrl("http", "staticsharp.github.io"),
                FilePath.FromOsPath(baseDirectory)
                );

            await generator.GenerateAsync(new αRoot(default));
        }
    }
}