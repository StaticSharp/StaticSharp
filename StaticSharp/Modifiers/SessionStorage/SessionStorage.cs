using StaticSharp.Gears;


namespace StaticSharp {
    public interface JSessionStorageNumber : JModifier {
        double StoredValue { get; }
        double ValueToStore { set; }
    }


    [ConstructorJs("SessionStorage")]
    public partial class SessionStorageNumber : Modifier {
        public required string Name { set; private get; }

        public override IdAndScript Generate(string elementId, Context context) {
            var result = base.Generate(elementId, context);
            result.Script.Add($"{result.Id}.name = \"{Name}\"");
            result.Script.Add($"{result.Id}.StoredValue = Number(sessionStorage.getItem(\"{Name}\") || 0) || 0");
            return result;
        }
    }

    


}


