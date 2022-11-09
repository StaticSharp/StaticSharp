namespace StaticSharpDemo.Root.Customization.HowToCreateNewComponent {

    [Representative]
    partial class Ru : Page {

        
        public override string Title => "Как создать новый компонент";
        public override Inlines DescriptionContent => $"Компоненты для создания страниц.";

        public override Blocks Content => new() {
            $"сначала напишу все в кучу, а потом правильно распределю..",
            //new Heading("SCSS"),
            $"Элемент отталкивается от предидущего. т.е. имеют только верхни margin",

            //new BillboardSolidColor { Color = Color.Violet },
            //new BillboardSolidColor { Color = Color.BlueViolet }

        };
    }


}
