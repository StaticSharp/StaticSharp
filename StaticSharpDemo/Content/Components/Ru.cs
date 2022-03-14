using System.Linq;

namespace StaticSharpDemo.Root.Components;

    [Representative]
    partial class Ru : Material {
        public override Paragraph Description => $"Компоненты для создания страниц.";

        public override Column Content => new() {

            new Button($"Hello"),

            new Heading("Создание нового компонента."),
            $"Если понадобится компонент, которого нет среди стандартных.",
            $"Можно создать компонент прям в проекте вашего сайта.",
            //new ToDo("пример кода"),
            $"Тут подробнее : {Node.Root.Customization.HowToCreateNewComponent}",
            


            Enumerable.Range(0,1000).Select(i => new Paragraph() {$"This is paragraph #{i}"}),


            /*new Heading("Billboards"),
            "Билборды - это блоки занимающие всю страницу. Между билбордами нет вертикальных отступов",
            new BillboardSolidColor { 
                Color = Color.LightBlue,
                Content = {
                    new Heading("BillboardSolidColor"){
                        Style = new {
                            FontSize = "60px"
                        }
                    },
                    "BillboardSolidColor.Content это коллекция текстового контента. Можно использовать заголовки и параграфы",
                    new OverlayLink(Node)
                }
            },
            new BillboardSolidColor {
                Color = Color.BlueViolet,
            },*/

            new Paragraph() { "Paragraph as link",new OverlayLink(Node.Root)},
            

            new Heading("Buttons"),

            
            

        };
    }

