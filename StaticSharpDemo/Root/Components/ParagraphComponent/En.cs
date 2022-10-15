namespace StaticSharpDemo.Root.Components.ParagraphComponent {

    [Representative]
    public partial class En : ComponentPage {
        public override Blocks? Content => new (){
            "Single line paragraph",

            """
            Multi
            line
            paragraph
            """,

            $"Single line paragraph with {Node} link",

            new Paragraph("""
            Multi
            line
            paragraph
            aligned to the right
            """){
                TextAlignmentHorizontal = TextAlignmentHorizontal.Right
            },

            new Paragraph("""
            Text justification
            including last line
            """){
                TextAlignmentHorizontal = TextAlignmentHorizontal.JustifyIncludingLastLine
            },

            "Multiple spaces |        | 8 spaces",

            "Long single line paragraph, that should wrap. Long single line paragraph, that should wrap. Long single line paragraph, that should wrap.",

            "    Line starting with 4 spaces",
            "\tLine starting with tab (tab = 4 spaces)",

            "Long paragraph separated by tabs\tLong paragraph separated by tabs\tLong paragraph separated by tabs\tLong paragraph separated by tabs\tLong paragraph separated by tabs\t",

        };
    }
}
