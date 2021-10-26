using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CsmlWeb {

    public interface IKey {
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

        /*public static string Calculate(object obj) {
            var type = obj.GetType();
            var stringBuilder = new StringBuilder();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var innerKeys = fields.Where(x => x.FieldType.IsAssignableTo(typeof(IKey))).Select(x => {
                var innerObject = x.GetValue(obj);
                return innerObject.GetType()
                    .GetProperty(nameof(IKey.Key), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .GetValue(innerObject).ToString();
            });
            stringBuilder.Append(type.Name);
            foreach(var field in fields.Select(x => x.GetValue(obj).ToString())) {
                stringBuilder.Append('\0' + field);
            }
            foreach(var key in innerKeys) {
                stringBuilder.Append('\0' + key);
            }
            return stringBuilder.ToString();
        }*/
    }
}