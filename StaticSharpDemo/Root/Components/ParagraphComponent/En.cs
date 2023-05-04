using System.Linq;

namespace StaticSharpDemo.Root.Components.ParagraphComponent {

    [Representative]
    public partial class En : ArticlePage {
        public override Inlines Description => $"Paragraph is a Block of text and other Inlines";
        public override Blocks Article => new (){

            CodeBlockFromThisFileRegion("simpleString"),
#region simpleString
            "Single line paragraph",
#endregion

            CodeBlockFromThisFileRegion("multilineString"),
#region multilineString
            """
            Multiline
            paragraph
            """,
#endregion            
            
            CodeBlockFromThisFileRegion("interpolatedString"),
#region interpolatedString
            $"Use interpolated strings to insert {Node.Root:link} or {Bold("styled text")}",
#endregion  

            SectionHeader("Alignment"),



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


            SectionHeader("Background"),

            new Paragraph("""
                If background color is set, margins are replaced by paddings.
                Default buinding for MarginLeft is (element.BackgroundColor != undefined) ? 0 : 10,
                Default buinding for PaddingLeft is (element.BackgroundColor != undefined) ? 10 : undefined
                """){
                BackgroundColor = Color.BlueViolet,
            },
            new Paragraph("""
                Paddings = 30
                """){
                BackgroundColor = Color.Purple,
                Paddings = 30
            },

            new Paragraph("""
                LongWordScaling_LongWordScaling_LongWordScaling
                """){
                FontSize = 50,
                BackgroundColor = Color.DeepPink,
                Paddings = 20
            },

            new Paragraph("""
                Huge_paddings
                """){
                FontSize = 50,
                BackgroundColor = Color.LightPink,
                PaddingsHorizontal = 300,
                PaddingsVertical = 10
            },

            new Paragraph("""
                .FillWidth()
                """){
                BackgroundColor = Color.RebeccaPurple,
            }.FillWidth(),

            new Paragraph("""
                .FillWidth().InheritHorizontalPaddings()
                """){
                BackgroundColor = Color.MediumPurple,
            }.FillWidth().InheritHorizontalPaddings(),


            new LayoutOverride{ 
                Child = new Paragraph("""
                    PaddingLeft = new(e=>Js.Num.Max(e.Parent.PaddingLeft, 20)),
                    PaddingRight = new(e=>Js.Num.Max(e.Parent.PaddingRight, 20)),
                    Resize to see difference with previous
                    """){
                    PaddingLeft = new(e=>Js.Num.Max(e.Parent.Parent.PaddingLeft, 10)),
                    PaddingRight = new(e=>Js.Num.Max(e.Parent.Parent.PaddingRight, 10)),
                    BackgroundColor = Color.BlueViolet,
                },
                OverrideX = 0,
                OverrideWidth = new(e=>e.Parent.Width)

            },


            

            /*Enumerable.Range(0,1000).Select(x=>
                new Block{ 
                    Width = 10+x,
                    Height = 10+x,
                    BackgroundColor = Color.FromGrayscale(x%4*0.2)
                }
                //new Paragraph($"Paragraph #{x}"){ }
            ),*/

        };
    }
}
