namespace StaticSharpDemo.Root.Components {

    [Representative]
    public partial class Ru : Material {
        public override Inlines Description => $"Компоненты для создания страниц.";

        public override Group Content => new(){

            new Flipper{
                First = new Video("-LF5M9nlFQs"),
                Second = new Column{
                    Children = {
                        H3("Video"),
                        $"""
                        Компонент Video пожет отображаться как iframe или как video tag, в зависимости от настроек
                        Подробнее {Node.VideoPlayer}
                        """
                    }
                }
            },


        };

    }
}

