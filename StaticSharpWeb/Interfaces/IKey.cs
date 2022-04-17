using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StaticSharpWeb {

    /*public interface IKey {
        string Key { get; }
    }

    public static class Key {

        public static string Calculate(Type container, params object[] state) {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(container.Name);
            foreach(var i in state) {
                if(i is IKey key) {
                    stringBuilder.Append('\0').Append(key.Key);
                } else {
                    stringBuilder.Append('\0').Append(i);
                }
            }
            return stringBuilder.ToString();
            
        }

    }*/
}