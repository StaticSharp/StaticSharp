using StaticSharp.Tree;
using System;
using System.Linq;

namespace StaticSharp {
    public interface INodeToPath {
        string NodeToRelativeDirectory(INode node);
        string NodeToRelativePath(INode node);//Starts with "/"

        string NodeToRelativeUrl(INode node) {
            return NodeToRelativePath(node);
        }
        string NodeToRelativeFilePath(INode node) {
            return NodeToRelativePath(node) + ".html";
        }
    }

    public class DefaultMultilanguageNodeToPath<LanguageEnum> : INodeToPath where LanguageEnum : struct, Enum {
        public string NodeToRelativeDirectory(INode node) {
            string path = string.Concat(node.Path.Select(x => '/' + x));
            return path;            
        }

        public string NodeToRelativePath(INode node) {
            string path = NodeToRelativeDirectory(node);
            if (node is MultilanguageProtoNode<LanguageEnum> protoNode) {
                return path + "/" + protoNode.Language.ToString().ToLower();
            } else {
                throw new Exception($"NodeToPath: {node.GetType()} is not {nameof(MultilanguageProtoNode<LanguageEnum>)}");
            }
        }
    }

}
