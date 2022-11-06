using StaticSharp.Tree;
using System;

namespace StaticSharp {
    public interface INodeToPath {
        string NodeToPath(INode node);

        string NodeToRelativeUrl(INode node) {
            return NodeToPath(node) + ".html";
        }
        string NodeToRelativeFilePath(INode node) {
            return NodeToPath(node) + ".html";
        }
    }

    public class DefaultMultilanguageNodeToPath<LanguageEnum> : INodeToPath where LanguageEnum : struct, Enum {
        public string NodeToPath(INode node) {
            if (node is MultilanguageProtoNode<LanguageEnum> protoNode) {
                string path;
                if (protoNode.Path.Length == 0) {//root
                    path = "Index";
                } else {
                    path = string.Join('/', protoNode.Path);
                }
                return path + "_" + protoNode.Language.ToString();

            } else {
                throw new Exception($"NodeToPath: {node.GetType()} is not {nameof(MultilanguageProtoNode<LanguageEnum>)}");
            }
        }
    }

}
