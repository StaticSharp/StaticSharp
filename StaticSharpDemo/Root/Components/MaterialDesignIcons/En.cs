using System;
using System.Linq;

namespace StaticSharpDemo.Root.Components.MaterialDesignIcons {

    [Representative]
    public partial class En : Material {
        
        public override Blocks? Content => new() {
            H5(nameof(StaticSharp.MaterialDesignIconBlock)),

            new Row{ 
                Children = { 
                    new Space(),
                    new MaterialDesignIconBlock(global::MaterialDesignIcons.IconName.Github),
                    new MaterialDesignIconBlock(global::MaterialDesignIcons.IconName.Facebook),
                    new MaterialDesignIconBlock(global::MaterialDesignIcons.IconName.Twitter),
                    new MaterialDesignIconBlock(global::MaterialDesignIcons.IconName.Youtube),
                    new MaterialDesignIconBlock(global::MaterialDesignIcons.IconName.Vimeo),
                    new Space(),
                }
            }.Modify((Action<Row>)(x=>{
                foreach (var i in x.Children.Values.OfType<Block>()){
                    i.Paddings = 6;
                }
            })),

            H5(nameof(StaticSharp.MaterialDesignIconInline)),
            $"This -> {new MaterialDesignIconInline(global::MaterialDesignIcons.IconName.Github)} is as SVG icon",


            $"{(new MaterialDesignIconInline(global::MaterialDesignIcons.IconName.Github))}"

        };
    }


}
