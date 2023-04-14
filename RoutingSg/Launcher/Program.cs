public class Program
{
    public static async Task Main()
    {
        //var targetProjectPath = Path.Combine(ProjectDirectory.Path, "..\\TestProject\\RoutingSg.TestProject.csproj");
        var targetProjectPath = Path.Combine(ProjectDirectory.Path, "..\\..\\StaticSharp\\StaticSharpDemo\\StaticSharpDemo.csproj");
        var outputPath = Path.Combine(Path.GetDirectoryName(targetProjectPath), $".generated/{typeof(RoutingSg.RoutingSg).FullName}");
        outputPath += "_detached";
        await RoslynSourceGeneratorLauncher.RoslynSourceGeneratorLauncher.Launch(new RoutingSg.RoutingSg(), targetProjectPath, outputPath);
    }
}