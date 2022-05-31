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
                FontSize = new((element) => element.Sibling<SliderJs>("Slider").Value + 2),
                Children = { $"Modifier test" }
            },



            {
                "Slider",
                new Slider {
                    Min = 10,
                    Max = 50,
                    Id = "Slider"
                }
            },

            $"A B C D {4}",
            new Space(),

            $"Bold: {new InlineModifier { FontStyle = new FontStyle { Weight = FontWeight.Bold }, Children = { "Text" } }}",
            $"Если   понадобится компонент,которого нет среди стандартных.",
            $"Можно создать компонент прям в проекте вашего сайта."
        };
    }
}
