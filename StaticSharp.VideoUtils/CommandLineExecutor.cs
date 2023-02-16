using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharp.VideoUtils
{
    public static class CommandLineExecutor // TODO: move to lib
    {
        public static string ExecuteCommand(string command, string args, Dictionary<string, string>? envVariables = null)
        { 
            // TODO: verify command exists, and executed fine. Condider not installed SVN, dotnet run (dotnet.sdk)

            using (var process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = args;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                if (envVariables != null)
                {
                    foreach(var envVariable in envVariables)
                    {
                        process.StartInfo.EnvironmentVariables.Add(envVariable.Key, envVariable.Value);
                    }
                }

                process.Start();

                var reader = process.StandardOutput;
                var output = reader.ReadToEnd();

                var errorReader = process.StandardError;
                var errors = errorReader.ReadToEnd();

                process.WaitForExitAsync().Wait();

                return output;
            }
        }


        public static async Task ExecuteCommandAsync(string command, string args, Func<string, Task> stdOutputCallback, Dictionary<string, string>? envVariables = null)
        { 
            using var process = new Process();

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = args;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            if (envVariables != null)
            {
                foreach(var envVariable in envVariables)
                {
                    process.StartInfo.EnvironmentVariables.Add(envVariable.Key, envVariable.Value);
                }
            }

            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    // Result not awaited deliberately
                    Task.Run(async () =>
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(e.Data))
                            {
                                await stdOutputCallback(e.Data);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Exception: {ex.Message}"); // TODO: log error
                        }
                    }).Wait(); // !!!! Wait added later, also deliberately
                });

            // TODO: reuse std out
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
                 {
                    // Result not awaited deliberately
                    Task.Run(async () => {
                        try
                        {
                            if (!String.IsNullOrEmpty(e.Data))
                            {
                               await stdOutputCallback(e.Data);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Exception: {ex.Message}"); // TODO: log error
                        }
                    }).Wait(); // !!!! Wait added later, also deliberately;
                 });

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();
            Console.WriteLine("PROCESS EXITED");
            process.Close();
            Console.WriteLine("PROCESS CLOSED");
        }
    }
}