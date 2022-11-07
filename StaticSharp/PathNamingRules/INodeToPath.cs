using StaticSharp.Tree;
using System;
using System.Linq;

namespace StaticSharp {
    public interface INodeToPath {
        string NodeToPath(INode node);//Starts with "/"

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
                if (protoNode.Path.Length == 0) { 
                }
                string path = string.Concat(protoNode.Path.Select(x=>'/'+x));                
                return path + "/" + protoNode.Language.ToString();

            } else {
                throw new Exception($"NodeToPath: {node.GetType()} is not {nameof(MultilanguageProtoNode<LanguageEnum>)}");
            }
        }
    }

}
