
using NUglify.JavaScript.Syntax;
using StaticSharp.Resources.Text;

namespace StaticSharpDemo.Root.Modifiers {




    [Representative]
    partial class Modifiers_En : Modifiers_Common {
        public override Inlines? Description => $"""
            Modifiers are additional objects for extending components.
            Three groups can be distinguished:
            For decorating: {Code(nameof(Cursor))},
            Working with events: {Code(nameof(Hover))}, {Code(nameof(Button))}, {Code(nameof(Toggle))}
            Hints for other objects, for example, Flex adds hints for {Code(nameof(LinearLayout))}
            """;


        public override Blocks Article => new(){

            SectionHeader(nameof(Button)),
            BottonExample,


            SectionHeader(nameof(Toggle)),
            ToggleExample,

            SectionHeader(nameof(Hover)),
            HoverExample,


            SectionHeader(nameof(Cursor)),
            CursorExample,


            SectionHeader(nameof(BorderRadius)),
            BorderRadiusExample,

            SectionHeader(nameof(MaterialShadow)),
            MaterialShadowExample,
            
            SectionHeader(nameof(UserSelect)),
            UserSelectExample,

            SectionHeader("All together"),
            UsageExample,

            SectionHeader(nameof(BackgroundImage)),
            BackgroundImageExample_Simple,
            BackgroundImageExample_Complex,

            SectionHeader(nameof(LinearGradient)),
            $"""
            The {TypeName<LinearGradient>()} modifier has a {PropertyName("Keys")} property that defines the colors to use in the gradient. In this case, the gradient will start with the color OrangeRed, then transition to Orange at the 25% mark of the gradient, and finally end with MediumVioletRed.
            The Keys property in the LinearGradient modifier can take two types of values: either a single {TypeName<Color>()} object, as seen with the first and last keys in the example, or a pair of values {Code($"({TypeName<Color>()} color,{TypeName<double>()} position)")} where the second value is a percentage representing the position of the color in the gradient, as seen with the second key in the example. This pair of values can be thought of as a color-stop or color-key for the gradient.
            """,

            LinearGradientExample_Simple,

            $"""

            The {PropertyName("Angle")} property sets the direction of the gradient and is represented as a fraction of a full turn, measured clockwise.
            A value of 0 represents an upward gradient,
            0.25 - rightward,
            0.5 - downward,
            0.75 - leftward.

            All parameters of the LinearGradient modifier, including colors, positions, and the angle, are {Bold("bindings")}. This means that instead of setting fixed values, it's possible to use expressions that can change dynamically or respond to events. For example, by binding the color-stop positions to the {Code("Toggle.Value")} property over an animation function, the positions of the keys will change when the user clicks on the element, creating an interactive and visually dynamic effect.
            """,

            LinearGradientExample_Complex,

            SectionHeader(nameof(Outline)),
            OutlineExample,



            SectionHeader("Backdrop filters"),

            new Paragraph(TextUtils.LoremIpsum(10)){ 
                FontSize = 50,
                BackgroundColor = Color.Orange,
                UnmanagedChildren = {
                    new LinearLayout{
                        Vertical = new(e=>e.Height>e.Width),
                        Paddings = 20,                        
                        ItemGrow = 1,
                        GapGrow = 1,
                        StartGapGrow = 1,
                        EndGapGrow = 1,
                        Children = {
                            new Block{
                                Width = 100, Height = 100,
                                Modifiers = {
                                    new BackdropBlur {
                                        Radius = 10
                                    },
                                }
                            },
                            new Block{
                                Width = 100, Height = 100,
                                Modifiers = {
                                    new BackdropGrayscale {
                                        Amount = 1
                                    }
                                }
                            }
                        },
                    }.FillWidth().FillHeight()
                }
            },

            

        };
            

    }
}