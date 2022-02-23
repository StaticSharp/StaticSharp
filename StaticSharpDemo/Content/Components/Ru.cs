namespace StaticSharpDemo.Content.Components {

    [Representative]
    partial class Ru : Material {
        public override Paragraph Description => $"Компоненты для создания страниц.";

        public override MaterialContent Content => new() { 
            
            new Heading("Создание нового компонента."),
            $"Если понадобится компонент, которого нет среди стандартных. Например {new ToDo("придумать пример")}",
            $"Можно создать компонент прям в проекте вашего сайта.",
            //new ToDo("пример кода"),
            $"Тут подробнее : {Node.Root.Customization.HowToCreateNewComponent}",
            
            new Heading("Billboards"),
            "Билборды - это блоки занимающие всю страницу. Между билбордами нет вертикальных отступов",
            new BillboardSolidColor { 
                Color = Color.LightBlue,
                Content = {
                    new Heading("BillboardSolidColor"){
                        Style = new {
                            FontSize = "60px"
                        }
                    },
                    "BillboardSolidColor.Content это коллекция текстового контента. Можно использовать заголовки и параграфы"
                }
            }
            
        };
    }

}