namespace RoutingSg.Src.Helpers {

    /// <summary>
    /// Keywords, include string so search in code and strigns that will be written to generated code
    /// </summary>
    public static class Kw {
        // Keywords to write (outgoing contract)
        public const string α = "α";
        public readonly static string AlphaRoot = α + Root;
        public const string VirtualNodePropertyName = "VirtualNode";
        public const string NodePropertyName = "Node";
        public const string Representative = "Representative";
        public const string Representatives = "Representatives";
        public readonly static string Protonode = string.Format(MultilanguageProtonodeTemplate, Language);

        // Keywords to search (and write) (incomming contract)
        // In StaticSharp.Core
        public const string Page = "StaticSharp.Page";
        public const string Node = "StaticSharp.Tree.Node";
        public const string IRepresentative = "StaticSharp.Tree.IRepresentative";
        const string MultilanguageProtonodeTemplate = "global::StaticSharp.MultilanguageProtoNode<{0}>";

        // In user code
        public const string Root = "Root";
        public const string Language = "Language";
    }
}
