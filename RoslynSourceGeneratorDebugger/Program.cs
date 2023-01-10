using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using MixinGenerator;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Build.Locator;
using System.Runtime.CompilerServices;
using System.IO;
using System.Threading;

public class Program
{
    private static async Task Main(string[] args)
    {
        MSBuildLocator.RegisterDefaults();

        var generator = new MixinGenerator.MixinGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        var projectPath = Path.GetFullPath($"{GetPath()}/../StaticSharp/StaticSharp.csproj");
        var workspace = MSBuildWorkspace.Create();
        var project = await workspace.OpenProjectAsync(projectPath);
        var inputCompilation = await project.GetCompilationAsync();

        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

        var firstGeneratorResult = driver.GetRunResult().Results.FirstOrDefault();
        var generationResult = firstGeneratorResult.GeneratedSources.FirstOrDefault();

        var dataToSave = firstGeneratorResult.GeneratedSources.ToDictionary(_ => _.HintName, _ => _.SourceText.ToString());

        var outputDirectory = $"{GetPath()}/../StaticSharp/.generated/Debugger";
        var outputDirectoryInfo = Directory.CreateDirectory(outputDirectory);
        var filesToDelete = outputDirectoryInfo.GetFiles("*.cs", SearchOption.AllDirectories).Select(x => x.FullName).ToList();


        foreach (var r in dataToSave)
        {
            var outputPath = Path.Combine(outputDirectory, r.Key);
            filesToDelete.Remove(outputPath);
            File.WriteAllText(outputPath, r.Value);
        }

        foreach (var f in filesToDelete)
        {
            File.Delete(f);
        }

        
    }

    private static string GetPath([CallerFilePath] string path = "")
    {
        return System.IO.Path.GetDirectoryName(path);
    }
}