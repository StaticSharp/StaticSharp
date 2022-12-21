using StaticSharp.Gears;

namespace StaticSharpDemo.Root {
    public abstract partial class LandingPage: StaticSharp.Page {

        public override Genome<IAsset>? Favicon => LoadFile("favicon.svg");

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
                ScrollY = new(e=>Js.Storage.Restore("MainScroll", () => e.ScrollYActual)),
                //FontSize = new(e =>Js.Math.First( Js.Storage.Restore("FontSize", () => 10)),
                Content = new Column{
                    Width = new(e=>e.ParentBlock.Width),
                    PaddingsHorizontal = new(e=>Js.Math.Max(e.ParentBlock.Width-ColumnWidth , 0)/2),
                    Children = {
                        Menu,
                        Content
                    }                
                }.CenterHorizontally()           
            }.FillWidth().FillHeight()           
        };
    }
}
