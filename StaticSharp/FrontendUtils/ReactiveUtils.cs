namespace StaticSharp.Scripts {
    public class ReactiveUtilsAttribute : ScriptReferenceAttribute {
        public ReactiveUtilsAttribute() : base(GetScriptFilePath()) {
        }
    }
}