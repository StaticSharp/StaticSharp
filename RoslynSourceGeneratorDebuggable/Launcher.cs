using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;

using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis;
using System.Xml;
using Exo.RoslynSourceGeneratorDebuggable;
using System.Threading.Tasks;

namespace Exo.RoslynSourceGeneratorDebuggable {
    public class Launcher {
        /*public static string GetProjectPath(string solutionDirectory) {
            //var solutionDirectory = Path.GetFullPath(Path.Combine(ProjectDirectory.Path, ".."));
            var soutionFilePath = Directory.GetFiles(solutionDirectory, "*.sln")[0];
            var solutionFile = SolutionFile.Parse(soutionFilePath);

            var projects = solutionFile.ProjectsInOrder.Where(x => {
                XmlDocument project = new XmlDocument();
                project.LoadXml(File.ReadAllText(x.AbsolutePath));
                var node = project.SelectSingleNode("/Project/PropertyGroup/OutputType");
                if (node == null) return false;
                return node.InnerText.ToLower() == "exe";
            }).ToArray();


            return projects[0].AbsolutePath;
        }*/


        public static async Task<Compilation> GetCompilationAsync(string projectPath) {
            var workspace = MSBuildWorkspace.Create();

            var project = await workspace.OpenProjectAsync(projectPath);

            //project = project.WithAnalyzerReferences(Enumerable.Empty<AnalyzerReference>());

            

            var result = await project.GetCompilationAsync();

            return result;
        }

        public static async Task Main() {
            MSBuildLocator.RegisterDefaults();


            var projectPath = Environment.GetEnvironmentVariable("ProjectPath");
            projectPath = Path.GetFullPath(projectPath);
            //TODO: check. 

            var compilation = await GetCompilationAsync(projectPath);


            /*

            List<FileInfo> files = new List<FileInfo>();

            DirectoryInfo directoryInfo = new DirectoryInfo(testProjectDirectory);
            files.AddRange(directoryInfo.GetFiles("*.cs", SearchOption.TopDirectoryOnly));
            foreach (var d in directoryInfo.GetDirectories("*", SearchOption.TopDirectoryOnly)) {
                if (d.Name.StartsWith(".")) continue;
                if (d.Name == "obj") continue;
                if (d.Name == "bin") continue;
                files.AddRange(d.GetFiles("*.cs", SearchOption.AllDirectories));
            }

            var syntaxTrees = files.Select(x=> CSharpSyntaxTree.ParseText(File.ReadAllText(x.FullName)));
            */

            
            var outputDirectoryRoot = Path.Combine(Path.GetDirectoryName(projectPath), ".generated");

            var assembly = Assembly.GetEntryAssembly();
            var types = assembly.GetTypes();

            var generatorsTypes = Assembly.GetEntryAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(SourceGeneratorDebuggable)));



            foreach (var type in generatorsTypes) {
                var outputDirectory = Path.Combine(outputDirectoryRoot, type.FullName);
                CreateDirectory(outputDirectory);

                var filesToDelete = new DirectoryInfo(outputDirectory).GetFiles("*.cs", SearchOption.AllDirectories).Select(x => x.FullName).ToList();


                var generator = (SourceGeneratorDebuggable)Activator.CreateInstance(type);
                var initializationContext = new GeneratorInitializationContextDebug();
                generator.Initialize(initializationContext);
                var executionContext = new GeneratorExecutionContextDebug(initializationContext, compilation);

                generator.Execute(executionContext);

                foreach (var r in executionContext.Results) {
                    var outputPath = Path.Combine(outputDirectory, r.Key);
                    filesToDelete.Remove(outputPath);
                    File.WriteAllText(outputPath, r.Value);
                }

                foreach (var f in filesToDelete) {
                    File.Delete(f);
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

        /*static void ClearDirectory(string path) {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            foreach (FileInfo file in directoryInfo.GetFiles()) {
                file.Delete();
            }
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories()) {
                directory.Delete(true);
            }
        }*/


    }
}