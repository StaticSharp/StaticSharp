using System;
using System.Linq;

namespace StaticSharpDemo.Root.Components.MaterialDesignIconsComponent {

    [Representative]
    public partial class En : ComponentPage {
        public override Inlines Description => $"Material Design icon component. Has two implementations as Block and as Inline.";
        public override Blocks? Content => new() {
            H5(nameof(SvgIconBlock)),

            new Row{ 
                Children = { 
                    new Space(),
                    new SvgIconBlock(Icons.MaterialDesignIcons.Github),
                    new SvgIconBlock(Icons.MaterialDesignIcons.Facebook),
                    new SvgIconBlock(Icons.MaterialDesignIcons.Twitter),
                    new SvgIconBlock(Icons.MaterialDesignIcons.Youtube),
                    new SvgIconBlock(Icons.MaterialDesignIcons.Vimeo),
                    new Space(),
                }
            }.Modify(x=>{
                foreach (var i in x.Children.Values.OfType<Block>()){
                    i.Paddings = 6;
                }
            }),

            H5(nameof(SvgIconInline)),
            $"This -> {new SvgIconInline(Icons.MaterialDesignIcons.Github)} is as SVG icon",
            $"Scale = 2 {new SvgIconInline(Icons.MaterialDesignIcons.Github){
                ForegroundColor = Color.Red,
                Scale = 2 }}",
            $"BaselineOffset = 0 {new SvgIconInline(Icons.MaterialDesignIcons.Github){ BaselineOffset = 0 }}"
            

        };
    }


}
