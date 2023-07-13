namespace StaticSharp {

    public static class Configuration {

        public class Exception : System.Exception {
            public Exception(string variableName) : base($"Environment variable {variableName} is not set. Edit Properties/launchSettings.json") {
            }
        }

        public static string GetVariable(string variableName) {
            var result = Environment.GetEnvironmentVariable(variableName);
            if (result == null) {
                throw new Exception(variableName);
            }
            return result;
        }

        public static string GetVariable(string variableName, string defaultValue) {
            var result = Environment.GetEnvironmentVariable(variableName);
            if (result == null) {
                return defaultValue;
            }
            return result;

        }

    }


    
}
