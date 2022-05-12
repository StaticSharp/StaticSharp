namespace StaticSharp {



    public class BorderJs : SymbolJs {
        public NumberJs Left => new($"{value}.Left");
        public NumberJs Right => new($"{value}.Right");
        public NumberJs Top => new($"{value}.Top");
        public NumberJs Bottom => new($"{value}.Bottom");
        public BorderJs() { }
        public BorderJs(string value) : base(value) { }

    }
    


    public class Border<Js> : IReactiveObjectCs {

        public delegate T Expression<out T>(Js element);
        public Expression<NumberJs> Left { set; protected get; } = null!;
        public Expression<NumberJs> Right { set; protected get; } = null!;
        public Expression<NumberJs> Top { set; protected get; } = null!;
        public Expression<NumberJs> Bottom { set; protected get; } = null!;
    }
}