using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpDemo.Root {

    [Representative]
    public partial class Ru : Material{
        public override Column? Content => new() {
            $"A B C D",
            new Space(),
            $"Если   понадобится компонент,которого нет среди стандартных.",
            $"Можно создать компонент прям в проекте вашего сайта."
        };
    }
}
