namespace StaticSharp.Gears;

public class VSCode {
    public static bool Open(string filePath, int line) {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = $"/C code -g \"{filePath}:{line}\"";
        process.StartInfo = startInfo;
        var result =  process.Start();
        return result;
    }
}
