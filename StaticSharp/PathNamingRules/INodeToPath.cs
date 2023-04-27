using StaticSharp.Tree;
using System;
using System.Linq;

namespace StaticSharp
{
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

}
