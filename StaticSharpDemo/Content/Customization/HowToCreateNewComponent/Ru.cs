namespace StaticSharpDemo.Root.Customization.HowToCreateNewComponent {

    [Representative]
    partial class Ru : Material {
        public override string Title => "Как создать новый компонент";
        public override Paragraph Description => $"Компоненты для создания страниц.";

        public override Group Content => new() {
            $"сначала напишу все в кучу, а потом правильно распределю..",
            //new Heading("SCSS"),
            $"Элемент отталкивается от предидущего. т.е. имеют только верхни margin",

            //new BillboardSolidColor { Color = Color.Violet },
            //new BillboardSolidColor { Color = Color.BlueViolet }

        };
    }


}
