using StaticSharp.Tree;
using System;
using System.Linq;

namespace StaticSharp {
    public interface INodeToPath {
        FilePath NodeToRelativeDirectory(Node node);
        FilePath NodeToRelativePath(Node node);

        FilePath NodeToRelativeUrl(Node node) {
            return NodeToRelativePath(node);
        }
        FilePath NodeToRelativeFilePath(Node node) {
            var result = NodeToRelativePath(node);
            result.Items[result.Items.Length - 1] += ".html";
            return result;
        }
    }

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
