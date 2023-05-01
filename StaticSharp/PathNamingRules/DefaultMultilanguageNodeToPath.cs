using StaticSharp.Tree;

namespace StaticSharp
{
    public class DefaultMultilanguageNodeToPath<LanguageEnum> : INodeToPath where LanguageEnum : struct, Enum {
        public FilePath NodeToRelativeDirectory(Node node) {
            return new(node.Path);            
        }

        public FilePath NodeToRelativePath(Node node) {
            var path = NodeToRelativeDirectory(node);
            if (node is MultilanguageProtoNode<LanguageEnum> protoNode) {
                path += protoNode.Language.ToString().ToLower();
            }
            return path;
        }
    }

}
