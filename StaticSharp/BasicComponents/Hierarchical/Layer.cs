namespace StaticSharp {

    [Javascriptifier.JavascriptClass("")]
    public static class JsLayerExtension {

        [Javascriptifier.JavascriptOnlyMember]
        [Javascriptifier.JavascriptMethodFormat("{0}.Layer")]
        public static T GetLayer<T>(this T _this) where T : Js.Hierarchical => throw new Javascriptifier.JavascriptOnlyException();
    }

    namespace Scripts {
        public class LayerAttribute : ScriptReferenceAttribute {
            public LayerAttribute() : base(GetScriptFilePath()) {
            }
        }
    }



}