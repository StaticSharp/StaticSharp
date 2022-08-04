using System;
using System.Reflection;
using System.Text;

namespace StaticSharp;
/*
public interface IReactiveObjectCs {
    public string ToJsObject(SymbolJs context) {
        var script = new StringBuilder();

        foreach (var i in GetType().GetProperties()) {

            MethodInfo? getter = i.GetGetMethod(nonPublic: true);
            if (getter != null) {

                var type = getter.ReturnType;
                if (typeof(Delegate).IsAssignableFrom(type)) {

                    object? value = getter.Invoke(this, null);

                    //var value = i.GetValue(this, getValueBindingFlags, null, null, null);

                    if (value != null) {
                        if (value is Delegate d) {
                            var symbolObject = d.DynamicInvoke(context);
                            if (symbolObject is SymbolJs symbol) {
                                if (symbol.isConstant)
                                    script.Append($"{i.Name}:{symbol},");
                                else
                                    script.Append($"{i.Name}:()=>{symbol},");
                            }
                        } else {
                            if (value is IReactiveObjectCs obj) {
                                script.Append($"{i.Name}:{obj.ToJsObject(context)},");
                            }

                        }
                    }
                }
            }
        }

        if (script.Length == 0)
            return "";

        return "{" + script.ToString() + "}";
    }

}
*/