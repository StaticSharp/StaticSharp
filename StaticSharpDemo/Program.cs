
using StaticSharpDemo.Root;
using StaticSharp.Gears;
using StaticSharpWeb;
using System;
using System.Collections.Generic;



using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using StaticSharp.Tree;

namespace StaticSharpWeb {

    public interface IStaticGenerator /*: IUrls*/ {
        Uri BaseUrl { get; }
        string BaseDirectory { get; }
        string TempDirectory { get; }
    }

    
}

namespace StaticSharpDemo {


    internal class Program {

        private static async Task Main(string[] args) {

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


            //var generator = new Content.StaticGenerator(
            //        new Uri(@"D:/TestSite/"),
            //        new Storage(@"D:\TestSite", @"D:\TestSite\IntermediateCache"),
            //        @"D:\staticsharp.github.io"
            //);
            //await generator.GenerateAsync();


        }

        public static async Task Server() {
            Cache.Directory = AbsolutePath(".cache");

            await new StaticSharp.Server(
                new DefaultMultilanguagePageFinder<Language>((language) => new αRoot(language)),
                new DefaultMultilanguageNodeToPath<Language>()

                ).RunAsync();
        }

        public static async Task Generator() {
            Cache.Directory = AbsolutePath(".cache");

            var generator = new MultilanguageStaticGenerator<Language>(
                new DefaultMultilanguageNodeToPath<Language>(),
                new Uri("http://staticsharp.github.io"),
                Path.GetFullPath(Path.Combine(ProjectDirectory.Path, "../../StaticSharp.github.io"))
                );

            await generator.GenerateAsync(new αRoot(default));
        }
    }
}