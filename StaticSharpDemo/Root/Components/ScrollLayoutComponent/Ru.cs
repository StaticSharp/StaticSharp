using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpDemo.Root.Components.ScrollLayoutComponent {



    [Representative]
    public partial class Ru: ComponentPage {
        public override string Title => "Scroll|   Layout";
        public override Blocks? Content => new() { 
            new ScrollLayout(){ 
                Height = 300,
                Content = new MaterialDesignIconBlock(MaterialDesignIcons.IconName.Carrot){
                    Width = 1024,
                    BackgroundColor = Color.MediumPurple,
                }
                
            },
            new Slider(){ 
                
            }
        };
    }
}
