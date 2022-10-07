using System.Reflection;

namespace MaterialDesignIcons;

public static partial class Icon {

    static Assembly Assembly = typeof(Icon).Assembly;
    static string GetResourcePathFromName(string name) {
        var resourcePath = Assembly.GetName().Name + ".svg." + name + ".svg";
        return resourcePath;
    }

    public static string GetSvg(IconName iconName) {
        var name = iconName.ToString();
        return GetSvg(name);
    }
    public static string GetSvg(string iconName) {
        var resourcePath = GetResourcePathFromName(iconName);
        using var stream = Assembly.GetManifestResourceStream(resourcePath);
        if (stream == null)
            throw new InvalidOperationException($"Invalid icon name {iconName}");

        using var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8, true);

        var text = streamReader.ReadToEnd();
        return text;
    }

    /*public static async Task Main(string[] args) {        
        var result = GetSvg(IconName.Github);
    }*/
}