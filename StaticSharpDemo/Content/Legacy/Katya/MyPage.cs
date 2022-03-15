using StaticSharpEngine;
using StaticSharpWeb;
using System;
using StaticSharpWeb.Components;

namespace StaticSharpDemo.Root.Legacy.Katya {

    [Representative]
    partial class MyPage : Common {
        public override string Title => "asdasdasd";
        //public override IImage TitleImage => new Video("qj6S37xIqK0").ConfigureAsBackgroundVideo();

        public override Paragraph Description => new() { "Ссылка на эту статью: ", Node, " ААА ААА ААА" };        
        private Image image => new Image(AbsolutePath("222.jpg"), "Mountain");
        public override Column Content => new () {

            new Paragraph() { 
                "Тестовый параграф"
            },

            /*new Grid(ContentWidth / 4) {
                new MaterialCard(Node.Representative),
                new MaterialCard(Node.Representative),
                new MaterialCard(Node.Representative),
                new MaterialCard(Node.Representative),
                new MaterialCard(Node.Representative),
                new MaterialCard(Node.Representative),
                new MaterialCard(Node.Representative),
                new MaterialCard(Node.Representative),
                new MaterialCard(Node.Representative),
            },*/

            new Paragraph()
            {
                "-------------------------"
            },

            new Video("S_fyy8X8C1g"),
            new Video("S_fyy8X8C1g", image),
            new Paragraph() {
                "Еще один тестовый параграф."
            },

            new Paragraph()
            {
                "Еще еще еще один тестовый параграф."
            }
        };
    }
}