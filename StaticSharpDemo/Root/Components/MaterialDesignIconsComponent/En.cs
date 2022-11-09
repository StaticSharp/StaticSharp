using System;
using System.Linq;

namespace StaticSharpDemo.Root.Components.MaterialDesignIconsComponent {

    [Representative]
    public partial class En : ComponentPage {
        public override Inlines DescriptionContent => $"Material Design icon component. Has two implementations as Block and as Inline.";
        public override Blocks? Content => new() {
            H5(nameof(MaterialDesignIconBlock)),

            new Row{ 
                Children = { 
                    new Space(),
                    new MaterialDesignIconBlock(MaterialDesignIcons.IconName.Github),
                    new MaterialDesignIconBlock(MaterialDesignIcons.IconName.Facebook),
                    new MaterialDesignIconBlock(MaterialDesignIcons.IconName.Twitter),
                    new MaterialDesignIconBlock(MaterialDesignIcons.IconName.Youtube),
                    new MaterialDesignIconBlock(MaterialDesignIcons.IconName.Vimeo),
                    new Space(),
                }
            }.Modify(x=>{
                foreach (var i in x.Children.Values.OfType<Block>()){
                    i.Paddings = 6;
                }
            }),

            H5(nameof(MaterialDesignIconInline)),
            $"This -> {new MaterialDesignIconInline(MaterialDesignIcons.IconName.Github)} is as SVG icon",
            $"Scale = 2 {new MaterialDesignIconInline(MaterialDesignIcons.IconName.Github){
                ForegroundColor = Color.Red,
                Scale = 2 }}",
            $"BaselineOffset = 0 {new MaterialDesignIconInline(MaterialDesignIcons.IconName.Github){ BaselineOffset = 0 }}"


        };
    }


}
