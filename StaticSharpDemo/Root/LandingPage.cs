namespace StaticSharpDemo.Root {
    public abstract partial class LandingPage: StaticSharp.Page {
        public override string PageLanguage => Node.Language.ToString().ToLower();


        public virtual Block Menu => new Row {
            BackgroundColor = Color.Black,
            Children = {
                new MaterialDesignIconBlock(MaterialDesignIcons.IconName.PoundBoxOutline) {
                    Height = new(e=>e.Parent.Child<Js.Block>(2).Height)
                },
                new Space(),
                MenuItem(Node.Components),
            },
            
        }.FillWidth().InheritHorizontalPaddings();

        public abstract Blocks? Content { get; }

        public virtual double ColumnWidth => 1080;

        protected override Blocks BodyContent => new Blocks {
            new ScrollLayout{ 
                Content = new Column{
                    Width = new(e=>e.ParentBlock.Width),
                    PaddingsHorizontal = new(e=>Js.Math.Max((e.ParentBlock.Width-ColumnWidth)/2,0)),
                    Children = {
                        Menu,
                        Content
                    }                
                }.CenterHorizontally()           
            }.FillWidth().FillHeight()           
        };
    }
}
