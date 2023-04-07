
public class Program
{
    public static async Task Main()
    {
        var targetProjectPath = Path.Combine(ProjectDirectory.Path, "..\\..\\StaticSharp\\StaticSharp.csproj");
        var outputPath = Path.Combine(Path.GetDirectoryName(targetProjectPath), 
            $".generated/{typeof(StaticSharpComponentSourceGenerator.StaticSharpComponentSourceGenerator).FullName}");
        await RoslynSourceGeneratorLauncher.RoslynSourceGeneratorLauncher.Launch(
            new StaticSharpComponentSourceGenerator.StaticSharpComponentSourceGenerator(), targetProjectPath, outputPath);
    }
}