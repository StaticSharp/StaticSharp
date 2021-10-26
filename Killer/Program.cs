using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;


class Representative : Attribute { }

namespace Killer
{
    class Program
    {

        static int Width = 10;
        static int Depth = 3;

        static void Main(string[] args)
        {
            var outputDirectory = Path.Combine(ThisFileDirectory(), "Content");

            WriteFiles(outputDirectory, new string[] { "Killer" , "Content" });

            var c = new Killer.Content.Depth0_0.Depth1_0.Depth2_0.Class();

        }
        
        
        static void WriteFiles(string prefix, IEnumerable<string> path, int depth = 0) {

            string code =
@$"namespace {string.Join('.', path)}{{
    [Representative]
    partial record Class{{
    }}
}}";
            CreateDirectory(prefix);
            
            File.WriteAllText(Path.Combine(prefix, "Class.cs"), code);

            if (depth < Depth) {
                
                for (int i = 0; i < Width; i++) {
                    var name = $"Depth{depth}_{i}";

                    WriteFiles(Path.Combine(prefix, name), path.Append(name), depth+1);

                }

            
                
            }
        }



        public static void CreateDirectory(string directory, int timeoutMs = 2000) {

            var last = Path.GetDirectoryName(directory);
            if (!Directory.Exists(last)) CreateDirectory(last);
            DirectoryInfo directoryInfo = Directory.CreateDirectory(directory);

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            while (!directoryInfo.Exists) {
                if (stopwatch.ElapsedMilliseconds > timeoutMs)
                    throw new Exception($"Failed to create directory: {directory}");
                Thread.Sleep(10);
            }
        }
        public static string ThisFileDirectory([CallerFilePath] string path = "") {
            return Path.GetDirectoryName(path);

        }
    }
}
