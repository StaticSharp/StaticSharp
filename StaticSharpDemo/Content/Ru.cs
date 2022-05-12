using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpDemo.Root {

    [Representative]
    public partial class Ru : Material{
        public override Group? Content => new() {
            
            H1("H1"),
            new Modifier { 
                FontSize = (element)=> element.ById("Slider").As<SliderJs>().Value,
                Children = { $"Modifier test" }
            },

            new Slider { 
                Id = (_)=> "Slider"
            },

            $"A B C D {4}",
            new Space(),

            $"Bold: {new InlineModifier { FontStyle = new FontStyle{FontWeight = FontWeight.Bold}, Children = { "Text"} }}",
            $"Если   понадобится компонент,которого нет среди стандартных.",
            $"Можно создать компонент прям в проекте вашего сайта."
        };
    }
}
