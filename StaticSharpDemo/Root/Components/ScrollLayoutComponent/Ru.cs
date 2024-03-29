﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpDemo.Root.Components.ScrollLayoutComponent {



    [Representative]
    public partial class Ru: Page {
        public override string Title => "Scroll Layout";
        public override Inlines Description => $"Контейнер вертикальным и горизонтальным скроллбарами.";
        public override Blocks? Content => new() { 
            new ScrollView(){ 
                Height = 300,
                Child = new SvgIconBlock(SvgIcons.MaterialDesignIcons.Carrot){
                    Width = 1024,
                    BackgroundColor = Color.MediumPurple,
                }                
            },

            "Paddings:",
            new ScrollView(){
                Height = 200,
                Paddings = 20,
                BackgroundColor = Color.Pink,
                Child = new SvgIconBlock(SvgIcons.MaterialDesignIcons.Carrot){
                    Width = 1024,
                    BackgroundColor = Color.MediumPurple,
                }
            },

            new ScrollView(){
                BackgroundColor = Color.LightGray,
                Paddings = 10,
                //PaddingLeft = 50,
                Height = 200,
                UnmanagedChildren = { 
                    new SvgIconBlock(SvgIcons.MaterialDesignIcons.VectorDifferenceAb){ 
                        Width = 100
                    }
                },
                Child = new Paragraph($"""
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam eu mattis erat, vel rutrum orci. Integer eu tincidunt nisl, et eleifend nisl. Cras imperdiet suscipit mi, et tempus erat tristique vel. Pellentesque ornare, diam at imperdiet fermentum, augue velit rhoncus nunc, vehicula auctor augue turpis ac nunc. Sed faucibus lectus malesuada commodo mattis. Mauris dignissim purus vitae libero condimentum, vitae placerat quam sodales. Etiam ut erat eu nisi dignissim venenatis vitae vel enim.
                    In vitae nibh quis nunc luctus scelerisque. Vivamus molestie porta orci, eget vestibulum tortor cursus nec. Praesent sed volutpat sapien, nec maximus nisi. Donec congue ultrices convallis. Etiam nec sapien vel sem eleifend convallis. Vivamus gravida, diam ac dignissim condimentum, tortor libero hendrerit nulla, vel rhoncus tellus risus ultricies nunc. In maximus auctor tempus. Aenean consequat ipsum vel tortor consectetur cursus. Maecenas sit amet ornare leo. Quisque vitae lacus efficitur, efficitur magna eu, sollicitudin ante. Mauris vulputate eleifend nisi eu laoreet. Sed sit amet lectus eu turpis pharetra faucibus in vitae massa. Integer elementum interdum leo a ullamcorper.
                    Aenean sed erat efficitur, lobortis tortor vitae, maximus est. Phasellus id velit ut arcu gravida vulputate. Fusce sit amet turpis iaculis, posuere neque in, bibendum massa. Praesent fringilla dolor a vulputate feugiat. Maecenas velit sem, aliquet non tempus vitae, vulputate at felis. Pellentesque a erat at sapien molestie accumsan eget scelerisque arcu. In hendrerit ornare tortor sed vulputate.
                    Morbi neque sem, laoreet sollicitudin dui sed, fermentum euismod purus. Morbi nec malesuada mi. Cras placerat malesuada finibus. Nunc facilisis vulputate tellus eu condimentum. Mauris interdum malesuada quam eget ultrices. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Maecenas in maximus nunc. Donec neque eros, bibendum ut convallis at, mattis ac risus. Praesent consectetur, orci convallis euismod ullamcorper, est purus ultricies sapien, pharetra lobortis tortor erat ac purus. Nam molestie, nisl et iaculis commodo, ex ante tempor ipsum, vel fringilla dolor orci nec elit. Nullam et sapien vel lacus egestas mattis. Vestibulum cursus posuere mi et porta. Duis sed nibh ultricies, elementum sapien quis, ornare enim. Fusce laoreet metus non semper vehicula.
                    Interdum et malesuada fames ac ante ipsum primis in faucibus. Nam enim leo, feugiat sed ante non, ornare fringilla magna. Nullam maximus dignissim quam, eget vulputate risus imperdiet nec. Pellentesque vitae porttitor sapien. Etiam elementum malesuada purus in malesuada. Mauris lobortis, ipsum porttitor ornare iaculis, turpis augue rhoncus dui, at imperdiet turpis justo rutrum dui. Proin ac aliquet elit, ac lobortis sapien. In a tristique ante, at commodo magna. Nullam vel blandit mauris, vitae tempor arcu. Maecenas metus nulla, mollis finibus tristique pulvinar, pulvinar et felis. Sed dui orci, ornare vel iaculis ac, molestie ut dolor.
                    """){ 
                    MarginLeft = new(e=>e.Parent.UnmanagedChildren.First().Width)
                }
            },

            new ScrollView(){
                Height = 200,
                Child = new Paragraph($"""
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam eu mattis erat, vel rutrum orci. Integer eu tincidunt nisl, et eleifend nisl. Cras imperdiet suscipit mi, et tempus erat tristique vel. Pellentesque ornare, diam at imperdiet fermentum, augue velit rhoncus nunc, vehicula auctor augue turpis ac nunc. Sed faucibus lectus malesuada commodo mattis. Mauris dignissim purus vitae libero condimentum, vitae placerat quam sodales. Etiam ut erat eu nisi dignissim venenatis vitae vel enim.
                    In vitae nibh quis nunc luctus scelerisque. Vivamus molestie porta orci, eget vestibulum tortor cursus nec. Praesent sed volutpat sapien, nec maximus nisi. Donec congue ultrices convallis. Etiam nec sapien vel sem eleifend convallis. Vivamus gravida, diam ac dignissim condimentum, tortor libero hendrerit nulla, vel rhoncus tellus risus ultricies nunc. In maximus auctor tempus. Aenean consequat ipsum vel tortor consectetur cursus. Maecenas sit amet ornare leo. Quisque vitae lacus efficitur, efficitur magna eu, sollicitudin ante. Mauris vulputate eleifend nisi eu laoreet. Sed sit amet lectus eu turpis pharetra faucibus in vitae massa. Integer elementum interdum leo a ullamcorper.
                    Aenean sed erat efficitur, lobortis tortor vitae, maximus est. Phasellus id velit ut arcu gravida vulputate. Fusce sit amet turpis iaculis, posuere neque in, bibendum massa. Praesent fringilla dolor a vulputate feugiat. Maecenas velit sem, aliquet non tempus vitae, vulputate at felis. Pellentesque a erat at sapien molestie accumsan eget scelerisque arcu. In hendrerit ornare tortor sed vulputate.
                    Morbi neque sem, laoreet sollicitudin dui sed, fermentum euismod purus. Morbi nec malesuada mi. Cras placerat malesuada finibus. Nunc facilisis vulputate tellus eu condimentum. Mauris interdum malesuada quam eget ultrices. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Maecenas in maximus nunc. Donec neque eros, bibendum ut convallis at, mattis ac risus. Praesent consectetur, orci convallis euismod ullamcorper, est purus ultricies sapien, pharetra lobortis tortor erat ac purus. Nam molestie, nisl et iaculis commodo, ex ante tempor ipsum, vel fringilla dolor orci nec elit. Nullam et sapien vel lacus egestas mattis. Vestibulum cursus posuere mi et porta. Duis sed nibh ultricies, elementum sapien quis, ornare enim. Fusce laoreet metus non semper vehicula.
                    Interdum et malesuada fames ac ante ipsum primis in faucibus. Nam enim leo, feugiat sed ante non, ornare fringilla magna. Nullam maximus dignissim quam, eget vulputate risus imperdiet nec. Pellentesque vitae porttitor sapien. Etiam elementum malesuada purus in malesuada. Mauris lobortis, ipsum porttitor ornare iaculis, turpis augue rhoncus dui, at imperdiet turpis justo rutrum dui. Proin ac aliquet elit, ac lobortis sapien. In a tristique ante, at commodo magna. Nullam vel blandit mauris, vitae tempor arcu. Maecenas metus nulla, mollis finibus tristique pulvinar, pulvinar et felis. Sed dui orci, ornare vel iaculis ac, molestie ut dolor.
                    """){ 
                    Width = new(e=>e.Parent.Width)
                }
            },


        };
    }
}
