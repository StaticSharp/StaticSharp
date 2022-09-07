using System;
using System.Collections.Generic;
using System.Linq;

namespace StaticSharpDemo.Root.Components {



    [Representative]
    public partial class Ru : Material {
        public override Inlines Description => $"Компоненты для создания {8} страниц.";






        /*new Row().Modify(x => {
        foreach (var i in CreateParagraphs(1000))
            x.Add(i);
    }),*/

        public override Group Content => new() {

        "Text",

        //new Button($"Hello"),

        //new Item(),


        //new Heading("Создание нового компонента."),
        $"A B C D",
        new Space(),
        $"Если   понадобится компонент,которого нет среди стандартных.",
        $"Можно создать компонент прям в проекте вашего сайта.",

        //CreateParagraph(10000),

        /*
        //new ToDo("пример кода"),
        $"Тут подробнее : {Node.Root.Customization.HowToCreateNewComponent} текст после ссылки",


        //CreateParagraphs(),



        new Heading("Billboards"),
        "Билборды - это блоки занимающие всю страницу. Между билбордами нет вертикальных отступов",
        new BillboardSolidColor {
            new Heading("BillboardSolidColor"){
                Style = new {
                    FontSize = "60px"
                }
            },
            "BillboardSolidColor.Content это коллекция текстового контента. Можно использовать заголовки и параграфы",
            new OverlayLink(Node)
            
        },
        new BillboardSolidColor {
            //Color = Color.BlueViolet,
        },

        //new Paragraph() { "Paragraph as link", new OverlayLink(Node.Root) },


        new Heading("Buttons"),




        new Row() {
@"как выжил шерлок где ответы
купить китайский грузовик
как делать рыбные котлеты
кино россия боевик
актриса оттепель марьяна
актер играет дартаньяна
гвоздильный автомат станок
как подключается звонок
эстония анкета виза
старославянский алфавит
вид с космоса реальный вид
цветной советский телевизор
что входит в оливье салат
подарочный сертификат",

new Spacer(),

@"что ответить на кто он такой
ударение в слове доской
корпорация центр
гидрометеоцентр
что мне делать с вселенской тоской",

@"石室詩士施氏, 嗜獅, 誓食十獅。
氏時時適市視獅。
十時, 適十獅適市。
是時, 適施氏適市。
氏視是十獅, 恃矢勢, 使是十獅逝世。
氏拾是十獅屍, 適石室。
石室濕, 氏使侍拭石室。
石室拭, 氏始試食是十獅。
食時, 始識是十獅, 實十石獅屍。
試釋是事。"

        },

        new Row() {
            "A",new Spacer(1),"A",new Spacer(2),"A",new Spacer(1),"A",new Spacer(1),"A",new Spacer(1),"A",new Spacer(1),"B"
        }*/



    };
    }
}

