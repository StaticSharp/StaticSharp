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
                    new SvgIconBlock(SvgIcons.MaterialDesignIcons.Github),
                    new SvgIconBlock(SvgIcons.MaterialDesignIcons.Facebook),
                    new SvgIconBlock(SvgIcons.MaterialDesignIcons.Twitter),
                    new SvgIconBlock(SvgIcons.MaterialDesignIcons.Youtube),
                    new SvgIconBlock(SvgIcons.MaterialDesignIcons.Vimeo),
                    new Space(),
                }
            }.Modify(x=>{
                foreach (var i in x.Children.OfType<Block>()){
                    i.Paddings = 6;
                }
            }),

            H5(nameof(SvgIconInline)),
            $"This -> {new SvgIconInline(SvgIcons.MaterialDesignIcons.Github)} is as SVG icon",
            $"Scale = 2 {new SvgIconInline(SvgIcons.MaterialDesignIcons.Github){
                ForegroundColor = Color.Red,
                Scale = 2 }}",
            $"BaselineOffset = 0 {new SvgIconInline(SvgIcons.MaterialDesignIcons.Github){ BaselineOffset = 0 }}"
            

        };
    }


}
