using System;
using System.Reflection;
using System.Text;

namespace StaticSharp;

public interface IReactiveObjectCs {
    public string ToJson(Symbolic.Symbol context) {
        var script = new StringBuilder();

        foreach (var i in GetType().GetProperties()) {

            MethodInfo? getter = i.GetGetMethod(nonPublic: true);
            if (getter != null) {
                object? value = getter.Invoke(this, null);

                //var value = i.GetValue(this, getValueBindingFlags, null, null, null);

                if (value != null) {
                    if (value is Delegate d) {
                        var symbolObject = d.DynamicInvoke(context);
                        if (symbolObject is Symbolic.Symbol symbol) {
                            if (symbol.isConstant) 
                                script.Append($"{i.Name}:{symbol.value},");
                            else
                                script.Append($"{i.Name}:()=>{symbol.value},");
                        }
                    } else {
                        if (value is IReactiveObjectCs obj) {
                            script.Append($"{i.Name}:{obj.ToJson(context)},");
                        }

                    }
                }
            }
        }

        return "{" + script.ToString() + "}";
    }

}
