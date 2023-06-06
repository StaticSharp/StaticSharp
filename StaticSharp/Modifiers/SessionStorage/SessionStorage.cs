using StaticSharp.Gears;


namespace StaticSharp {
    public interface JSessionStorageNumber : JSessionStorage {
        double StoredValue { get; }
        double ValueToStore { set; }
    }



    public interface JSessionStorage : JModifier {
    }

    [ConstructorJs()]
    public abstract partial class SessionStorage : Modifier {
    
    }


    public partial class SessionStorageNumber : SessionStorage {
        public required string Name { set; private get; }

        public override IdAndScript Generate(string elementId, Context context) {
            var result = base.Generate(elementId, context);
            result.Script.Add($"{result.Id}.name = \"{Name}\"");
            result.Script.Add($"{result.Id}.StoredValue = Number(sessionStorage.getItem(\"{Name}\") || 0) || 0");
            return result;
        }
    }

    


}


