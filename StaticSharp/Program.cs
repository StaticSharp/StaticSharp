namespace StaticSharp {


    public abstract class Program {



        protected static async Task RunEntryPointFromEnvironmentVariable<TProgram>() {
            const string environmentVariableName = "entryPoint";

            var entryPointName = Environment.GetEnvironmentVariable(environmentVariableName);
            if (entryPointName == null) {
                Console.WriteLine($"EnvironmentVariable '{environmentVariableName}' not found.");
                return;
            }

            var entryPoint = typeof(TProgram).GetMethod(entryPointName,
                System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Static
                | System.Reflection.BindingFlags.FlattenHierarchy);

            if (entryPoint == null) {
                Console.WriteLine($"method {typeof(TProgram).FullName}.{entryPointName} not found.");
                return;
            }

            if (entryPoint.ReturnType == typeof(Task)) {
                var task = entryPoint.Invoke(null, null) as Task;
                if (task != null) {
                    await task;
                }
            } else {
                entryPoint.Invoke(null, null);
            }
        }
    }
}
