namespace StaticSharpDemo.Root.Components {

    [Representative]
    public partial class Ru : Material {

        protected override void Setup() {
            base.Setup();
            BackgroundColor = Color.FromArgb(0xffB56576);
        }
        public override Inlines Description => $"Компоненты для создания страниц.";

        public override Blocks Content => new(){

            new Flipper{
                First = new Video("-LF5M9nlFQs"),
                Second = new Column{
                    Children = {
                        new Space(),
                        H3("Video"),
                        $"""
                        Компонент Video пожет отображаться как iframe или как video tag, в зависимости от настроек
                        Подробнее {Node.VideoPlayer}
                        """,
                        new Space()
                    }
                }
            },
        };

    }
}

