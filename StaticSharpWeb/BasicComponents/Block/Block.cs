using StaticSharp.Gears;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    namespace Symbolic {
        public class Block : Object {
            public Block() { }
            public Block(string value) : base(value) {
            }

            public Block Parent => new($"{value}.Parent");
            public Number X => new($"{value}.X");
            public Number Y => new($"{value}.Y");
            public Number Width => new($"{value}.Width");
            public Number Height => new($"{value}.Height");
            public Border Margin => new($"{value}.Margin");

        }
    }

    public interface IBlock : IElement {
    }

    public abstract class Block<Js> : Reactive, IBlock where Js : Symbolic.Block, new() {

        public delegate T Expression<out T>(Js element);

        public string? Id = null;


        public Expression<Symbolic.Number> X { set; protected get; } = null!;
        public Expression<Symbolic.Number> Y { set; protected get; } = null!;

        public Expression<Symbolic.Number> Width { set; protected get; } = null!;
        public Expression<Symbolic.Number> Height { set; protected get; } = null!;

        //public Border<Js> Padding { set; protected get; } = null!;
        public Border<Js> Margin { set; protected get; } = null!;

        public Block(string callerFilePath, int callerLineNumber) : base(callerFilePath, callerLineNumber) { }

        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(AbsolutePath("Item.js")));
        }

        public string PropertiesInitializationScript() {
            return "element.Reactive = " + (this as IReactiveObjectCs).ToJson(new Js() { value = "element" });
        }

    }

    /*public sealed class Item : Item<Symbolic.Item> {
        public Item([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }
    }*/
}