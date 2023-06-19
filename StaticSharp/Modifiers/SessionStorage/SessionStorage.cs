using StaticSharp.Gears;
using StaticSharp.Html;

namespace StaticSharp {
    



    public interface JSessionStorage : JModifier {
    }

    [ConstructorJs()]
    public abstract partial class SessionStorage : Modifier {
    
    }


    public interface JSessionStorageNumber : JSessionStorage {
        double StoredValue { get; }
        double ValueToStore { set; }
    }

    [ConstructorJs()]
    public partial class SessionStorageNumber : SessionStorage {
        public required string Name { set; private get; }

        public override IdAndScript Generate(Tag element, Context context) {
            var result = base.Generate(element, context);
            result.Script.Add($"{result.Id}.name = \"{Name}\"");
            result.Script.Add($"{result.Id}.StoredValue = Number(sessionStorage.getItem(\"{Name}\") || 0) || 0");
            return result;
        }
    }

    
    public interface JSessionStorageBoolean : JSessionStorage {
        bool StoredValue { get; }
        bool ValueToStore { set; }
    }

    [ConstructorJs()]
    public partial class SessionStorageBoolean : SessionStorage {
        public required string Name { set; private get; }

        public override IdAndScript Generate(Tag element, Context context) {
            var result = base.Generate(element, context);
            result.Script.Add($"{result.Id}.name = \"{Name}\"");
            result.Script.Add($"{result.Id}.StoredValue = sessionStorage.getItem(\"{Name}\") === \"true\"");
            return result;
        }
    }



}


