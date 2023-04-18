namespace StaticSharpDemo.Root {
    public abstract partial class ArticlePage : Page {
        public override sealed Blocks? Content => new (){
            MainVisual,
            new Paragraph(Title){ 
                FontSize = 75
            },
            Description,
            Separator(),
            Article
        };

        public abstract Blocks Article { get; }

    }
}
