using StaticSharpEngine;
using StaticSharpWeb;
using StaticSharpWeb.Components;
using System.Drawing;
using StaticSharpWeb.Html;
using StaticSharpWeb.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpDemo.Content.Index {

    [Representative]
    partial class Ru : Common {
        public override string Title => null;
        //public override IImage TitleImage => new Video("qj6S37xIqK0").ConfigureAsBackgroundVideo();
        public override Paragraph Description => new() { "Ссылка на эту статью: ", Node, "ТЕСТ ТЕСТ ТЕСТ" };
        //private Image image = new Image(new RelativePath("111.jpg"), "Ricardooo");
        private Image image = new Image(new RelativePath("333.png"), "Ricardooo");


        //private Image refImage = new Image("https://bipbap.ru/wp-content/uploads/2017/04/0_7c779_5df17311_orig.jpg");

        //private Language wiki => new Language();
        //private Reference wiki_ru => new Reference("ТЕКСТ3: ", "https://ru.wikipedia.org/wiki/C_Sharp", "C#");    

        public override MaterialContent Content => new() {
            new CSharpCode(System.IO.File.ReadAllText(new RelativePath("Ru.cs"))),
            new Downloadable(new RelativePath("Download")),
            new Downloadable(new RelativePath("MoreDownload")),
            new Downloadable(@"D:/svn/antilatency.com/.Releases", "Api", new SearchSequencer().SvnLikeRepository.SpecifyThePath("Api\\Api.ml.cs")),
            new Reference("https://ru.wikipedia.org/wiki/C_Sharp"),
            new Reference("https://ru.wikipedia.org/wiki/C_Sharp", "C#", "Ссылки с текстом: "),
            new Reference("https://ru.wikipedia.org/wiki/C_Sharp", "C#", "Ссылки с текстом и подсказкой: ", "Шарп"),
            image,
            //new Reference("https://ru.wikipedia.org/wiki/C_Sharp", "C#", image),
            // new Landing(image, new Paragraph() {
            //     "HELLO"
            // }, 10),

            // new Grid(ContentWidth / 4) {
            //         new MaterialCard(Node.Representative),
            //         new MaterialCard(Node.Representative),
            //         new MaterialCard(Node.Representative),
            //     },

            new Paragraph() {
                "AAAAAAA"
                },

            new UnorderedList() {
                    "aaa",
                    new UnorderedList() {
                        "bbb",
                        "ccc"
                    },
                    "ddd"
                },

            new OrderedList() {
                    "zzz",
                    "sss",
                    new OrderedList() {
                        "bbb",
                        "ooo"
                    }
                },

            new Paragraph() {
                    "Без заголовка"
                },

            new Table(2) {
                    "a", "b", "c", "d"
                },

            new Paragraph() {
                    "С заголовком"
                },

            new Table("A", "B", "C", "D") {
                    "a", "b", "c", "d", "e", "f", "g", "h"
                },

            new ColorSequenceCos(Color.FromArgb(255, 128, 64), Color.Blue, 1.792f),
            new ColorSequence() {
                    new(Color.Red, 0.3f),
                    new(Color.Black, 0.3f),
                    new(Color.Green, 0.3f),
                    new(Color.Black, 0.3f),
                    new(Color.Red, 0.3f),
                    new(Color.Black, 2.3f)
                },
            new Table("Отображение", "Описание") {
                    new ColorSequenceCos(Color.Black, Color.FromArgb(0xa8, 0x00, 0xff), 1.792f), "Loading - Первое состояние Alt при подаче питания или перезагрузке, происходит инициализация периферии и применение настроек.",
                    new ColorSequenceCos(Color.Black, Color.FromArgb(0x7f, 0xba, 0xd9), 1.792f), "Idle - Ожидание задачи.",
                    new ColorSequenceCos(Color.Black, Color.FromArgb(0x00, 0xff, 0x00), 1.792f), "Task running - Alt выполняет задачу. Это может быть задача трекинга, обращение к свойствам или любая другая доступная задача."
                },

            //new Panel("Default Panel"),
            new Info("Ситуации 7 и 9 - это не рабочий вариант. Работа сети в такой конфигурации будет крайне плоха из-за большого"),
            new Error("Error Panel"),
            new Bug("Bug Panel"),
            new Note("Note Panel"),
            new Success("Success Panel"),
            new Warning("Warning Panel"),
        };
    }
}