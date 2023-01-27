using StaticSharp.Gears;
using StaticSharp.Tree;

namespace StaticSharp {

    /*public class MultilanguageServer<LanguageEnum> :  Server  {

        

        


        public override Uri NodeToUrl(Uri baseUrl, INode node) {
            if (node is MultilanguageProtoNode<LanguageEnum> protoNode) {
                string path;
                if (protoNode.Path.Length == 0) {//root
                    path = "Index";
                } else {
                    path = string.Join('/', protoNode.Path);
                }
                return new Uri(baseUrl, path + "_" + protoNode.Language.ToString() + ".html");

            } else {
                throw new Exception($"ProtoNodeToUri. {node.GetType()} is not ProtoNode");
            }
        }
    }*/
}
