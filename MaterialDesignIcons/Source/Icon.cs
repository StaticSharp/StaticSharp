using System.Reflection;

namespace Icons;
public class Icon {

    static Assembly Assembly = typeof(Icon).Assembly;
    public float Width { get; init; }
    public float Height { get; init; }
    public string Path { get; init; }

    public Icon(string path, float width, float height) {
        Width = width;
        Height = height;
        Path = path;
    }
    string GetResourcePathFromName() {
        Assembly assembly = GetType().Assembly;
        var resourcePath = assembly.GetName().Name + ".svg." + Path + ".svg";
        return resourcePath;
    }
    public string GetSvg() {
        var resourcePath = GetResourcePathFromName();
        using var stream = Assembly.GetManifestResourceStream(resourcePath);
        if (stream == null)
            throw new InvalidOperationException($"Invalid icon name {Path}");

        using var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8, true);

        var text = streamReader.ReadToEnd();
        return text;
    }

}