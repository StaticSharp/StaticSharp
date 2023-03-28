namespace StaticSharp.Scripts {
    public class DeveloperModeAttribute : ScriptReferenceAttribute {
        public DeveloperModeAttribute() : base(GetScriptFilePath()) {
        }
    }
}