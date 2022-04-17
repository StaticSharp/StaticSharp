namespace StaticSharp {


    namespace Symbolic {
        public class Border : Symbol {
            public Number Left => new($"{value}.Left");
            public Number Right => new($"{value}.Right");
            public Number Top => new($"{value}.Top");
            public Number Bottom => new($"{value}.Bottom");
            public Border() { }
            public Border(string value) : base(value) { }

        }
    }


    public class Border<Js> : IReactiveObjectCs {

        public delegate T Expression<out T>(Js element);
        public Expression<Symbolic.Number> Left { set; protected get; } = null!;
        public Expression<Symbolic.Number> Right { set; protected get; } = null!;
        public Expression<Symbolic.Number> Top { set; protected get; } = null!;
        public Expression<Symbolic.Number> Bottom { set; protected get; } = null!;
    }
}