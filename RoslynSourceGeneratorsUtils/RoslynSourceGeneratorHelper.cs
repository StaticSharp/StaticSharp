using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RoslynSourceGeneratorsUtils
{
    public static class RoslynSourceGeneratorHelper
    {
        public static async Task GenerateAndSaveFilesForProject(ISourceGenerator generator, string targeProjectPath, string outputFilesPath)
        {
            MSBuildLocator.RegisterDefaults();
            var workspace = MSBuildWorkspace.Create();
            var project = await workspace.OpenProjectAsync(targeProjectPath);
            project = project.WithAnalyzerReferences(Enumerable.Empty<AnalyzerReference>());
            var inputCompilation = await project.GetCompilationAsync();

            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

            var firstGeneratorResult = driver.GetRunResult().Results.FirstOrDefault();
            var generationResult = firstGeneratorResult.GeneratedSources.FirstOrDefault();
            var dataToSave = firstGeneratorResult.GeneratedSources.ToDictionary(_ => _.HintName, _ => _.SourceText.ToString());

            var outputDirectoryInfo = Directory.CreateDirectory(outputFilesPath);
            var filesToDelete = outputDirectoryInfo.GetFiles("*.cs", SearchOption.AllDirectories).Select(x => x.FullName).ToList();

            foreach (var r in dataToSave)
            {
                var outputPath = Path.Combine(outputFilesPath, r.Key);
                filesToDelete.Remove(outputPath);
                File.WriteAllText(outputPath, r.Value);
            }

            foreach (var f in filesToDelete)
            {
                File.Delete(f);
            }
        }
    }
}
