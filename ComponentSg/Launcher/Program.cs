
public class Program {
    public static async Task Main() {
        var ProjectName = "StaticSharp";

        var targetProjectPath = Path.Combine(ProjectDirectory.Path, $"..\\..\\{ProjectName}\\{ProjectName}.csproj");
        var outputPath = Path.Combine(Path.GetDirectoryName(targetProjectPath), 
            $".generated/{typeof(ComponentSg.ComponentSg).FullName}");
        await RoslynSourceGeneratorLauncher.RoslynSourceGeneratorLauncher.Launch(
            new ComponentSg.ComponentSg(), targetProjectPath, outputPath);
    }
}