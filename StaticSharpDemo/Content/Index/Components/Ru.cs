namespace StaticSharpDemo.Content.Index.Components {

    [Representative]
    partial class Ru : Material {
        public override Paragraph Description => new Paragraph() { "Компоненты для создания страниц." };

        public override MaterialContent Content => new() { 
            new Heading("Создание нового компонента."),
            $"Если понадобится компонент, которого нет среди стандартных. Например {new ToDo("придумать пример")}",
            $"Можно создать компонент прям в проекте вашего сайта.",
            //new ToDo("пример кода"),
            $"Тут подробнее : {Node.HowToCreateNewComponent}"
            
            
        };
    }

}