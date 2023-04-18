
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
    }
}