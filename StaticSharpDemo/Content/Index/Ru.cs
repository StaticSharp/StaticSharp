﻿using StaticSharpEngine;
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
        //public override IImage TitleImage => new Video("qj6S37xIqK0");
        //public override IImage TitleImage => new Video("qj6S37xIqK0");
        public override Paragraph Description => new() { "Ссылка на эту статью: ", Node, "ТЕСТ ТЕСТ ТЕСТ" };
        private Image image = new Image(new RelativePath("333.png"), "Ricardooo1");
        private Image imagec = new Image(new RelativePath("111c.jpg"), "Ricardoooc");
        private Image image2 = new Image(new RelativePath("222.png"), "Ricardooo2");
        private Image image3 = new Image(new RelativePath("333.png"), "Ricardooo3");

        private Image test1 = new Image(new RelativePath("test1t.jpg"), "test1");
        private Image test2 = new Image(new RelativePath("test2.jpg"), "test2");
        private Image test3 = new Image(new RelativePath("test3.jpg"), "test3");
        private Image test4 = new Image(new RelativePath("test4.jpg"), "test4");
        private Image test6 = new Image(new RelativePath("test6.jpg"), "test6");
        private Image test7 = new Image(new RelativePath("test7.jpg"), "test7");


        //private Image refImage = new Image("https://bipbap.ru/wp-content/uploads/2017/04/0_7c779_5df17311_orig.jpg");

        //private Language wiki => new Language();
        //private Reference wiki_ru => new Reference("ТЕКСТ3: ", "https://ru.wikipedia.org/wiki/C_Sharp", "C#");    

        public override MaterialContent Content => new() {
            //new CSharpCode(System.IO.File.ReadAllText(new RelativePath("Ru.cs"))),
            new Downloadable(new RelativePath("Download")),
            new Downloadable(new RelativePath("MoreDownload")),
            new Downloadable(@"D:/svn/antilatency.com/.Releases", "Api", new SearchSequencer().SvnLikeRepository.SpecifyThePath("Api\\Api.ml.cs")),
            new Reference("https://ru.wikipedia.org/wiki/C_Sharp"),
            new Reference("https://ru.wikipedia.org/wiki/C_Sharp", "C#", "Ссылки с текстом: "),
            new Reference("https://ru.wikipedia.org/wiki/C_Sharp", "C#", "Ссылки с текстом и подсказкой: ", "Шарп"),
            // new Paragraph() {
            //     "SimpleImage"
            // },
            test1,
            // new Paragraph() {
            //     "SimpleImage"
            // },
            // imagec,
            //test2,
            // new Landing(test1, "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.",  
            // "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", Position.TopLeft, new int[4] {470, 150, 660, 350}),

            // new Landing(test2, "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.",  
            // "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", Position.TopLeft, new int[4] {1100, 600, 1600, 850}),

            // new Landing(test3, "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.",  
            // "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", Position.TopLeft, new int[4] {300, 500, 850, 1100}),

            // new Landing(test4, "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.",  
            // "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", Position.TopLeft, new int[4] {400, 200, 650, 330}),
            
            // new Landing(test7, "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.",  
            // "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", Position.TopLeft, new int[4] {500, 450, 1000, 1100}),


            //------//

            // new Landing(test6, "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.",  
            // "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", Position.TopLeft, new int[4] {470, 150, 660, 350}),

            // new Landing(image2, "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", 
            // "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", Position.BottomLeft),
            // new Landing(image3, "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.",
            // "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", Position.TopRight),
            // new Landing(image2, "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.",
            // "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", Position.TopLeft),
            // new Landing(image3, "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.",
            // "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", Position.Top),
            // new Landing(image3, "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.",
            // "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", Position.Bottom),
            
            // new Landing(image3, "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.",
            // "Сделайте вход только по приглашениям, чтобы чувствовать себя комфортно.", Position.FullCenter),
            //new Reference("https://ru.wikipedia.org/wiki/C_Sharp", "C#", image),

            // new Grid(ContentWidth / 4) {
            //         new MaterialCard(Node.Representative),
            //         new MaterialCard(Node.Representative),
            //         new MaterialCard(Node.Representative),
            //     },

            // new Paragraph() {
            //     "AAAAAAA"
            //     },

            // new UnorderedList() {
            //         "aaa",
            //         new UnorderedList() {
            //             "bbb",
            //             "ccc"
            //         },
            //         "ddd"
            //     },

            // new OrderedList() {
            //         "zzz",
            //         "sss",
            //         new OrderedList() {
            //             "bbb",
            //             "ooo"
            //         }
            //     },

            // new Paragraph() {
            //         "Без заголовка"
            //     },

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