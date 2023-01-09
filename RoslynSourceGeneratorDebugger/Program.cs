using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using MixinGenerator;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Build.Locator;
using System.Runtime.CompilerServices;

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

        var result = driver.GetRunResult();
        var generationResult = result.Results.FirstOrDefault().GeneratedSources.FirstOrDefault();

        Console.WriteLine(generationResult.HintName);
    }

    private static string GetPath([CallerFilePath] string path = "")
    {
        return System.IO.Path.GetDirectoryName(path);
    }
}