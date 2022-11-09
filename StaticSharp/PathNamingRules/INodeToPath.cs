using StaticSharp.Tree;
using System;
using System.Linq;

namespace StaticSharp {
    public interface INodeToPath {
        string NodeToRelativeDirectory(Node node);
        string NodeToRelativePath(Node node);//Starts with "/"

        string NodeToRelativeUrl(Node node) {
            return NodeToRelativePath(node);
        }
        string NodeToRelativeFilePath(Node node) {
            return NodeToRelativePath(node) + ".html";
        }
    }

    public class DefaultMultilanguageNodeToPath<LanguageEnum> : INodeToPath where LanguageEnum : struct, Enum {
        public string NodeToRelativeDirectory(Node node) {
            string path = string.Concat(node.Path.Select(x => '/' + x));
            return path;            
        }

        public string NodeToRelativePath(Node node) {
            string path = NodeToRelativeDirectory(node);
            if (node is MultilanguageProtoNode<LanguageEnum> protoNode) {
                return path + "/" + protoNode.Language.ToString().ToLower();
            } else {
                throw new Exception($"NodeToPath: {node.GetType()} is not {nameof(MultilanguageProtoNode<LanguageEnum>)}");
            }
        }
    }

}
