
namespace StaticSharpDemo.Root.Modifiers {



    [Representative]
    partial class Modifiers_Ru : Modifiers_Common {
        public override Inlines? Description => $"""
            Модификаторы - это дополнительные объекты для расширения компонентов.
            Можно выделить три группы:
            Для декорирования: {Code(nameof(Cursor))},
            Работа с событиями: {Code(nameof(Hover))}, {Code(nameof(Button))}, {Code(nameof(Toggle))}
            Подсказки для других объектов, например, Flex добавляет подсказки для {Code(nameof(LinearLayout))}
            """;

    }
}
