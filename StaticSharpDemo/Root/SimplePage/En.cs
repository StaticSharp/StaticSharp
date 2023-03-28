namespace StaticSharpDemo.Root.SimplePage {



    [Representative]
    public partial class En : Page {
        public override Inlines Description => "This page is an example of syntax of StaticSharp";
        public override Blocks? Content => new() {            
            H1(Title),
            Description,

            $"Here is {Bold("bold")} text. This is {Node.Root:internal link}. And an icon : {new SvgIconInline(SvgIcons.SimpleIcons.GitHub)}",

            new LinearLayout{ 
                Vertical = new(e => e.Parent.Width < 480),
                Children = { 
                    new Block{ 
                        Width = 200,
                        Height = 200,
                        BackgroundColor = Color.BlueViolet
                    },
                    new Block{
                        Width = 200,
                        Height = 200,
                        BackgroundColor = Color.PaleVioletRed
                    },
                }
            },
            

            $"This is {Code(nameof(LinearLayout))} which is both Row and Column. In this example it is a Column if {Code("Parent.Width < 480")} and Row otherwise."
        };
    }
}