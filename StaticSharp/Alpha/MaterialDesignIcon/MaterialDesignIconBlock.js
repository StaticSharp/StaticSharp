function MaterialDesignIconBlock(element) {
    Block(element)



    element.Reactive = {

        Aspect: element.dataset.width / element.dataset.height,

        VerticalPaddingSum: () => Sum(element.PaddingTop, element.PaddingBottom),
        HorizontalPaddingSum: () => Sum(element.PaddingLeft, element.PaddingRight),

        InternalWidth: () => First((element.Height - element.VerticalPaddingSum) * element.Aspect + element.HorizontalPaddingSum, Sum(element.dataset.width, element.HorizontalPaddingSum)),
        InternalHeight: () => First((element.Width - element.HorizontalPaddingSum) / element.Aspect + element.VerticalPaddingSum, Sum(element.dataset.height, element.VerticalPaddingSum)),
    }


    new Reaction(() => {
        let content = element.children[0]
        let w = element.Width - element.HorizontalPaddingSum
        let h = element.Height - element.VerticalPaddingSum
        if (w <= 0 || h <= 0) {
            content.style.display = "none"
            content.style.width = ""
            content.style.height = ""
        } else {
            content.style.display = ""
            content.style.width = ToCssSize(w)
            content.style.height = ToCssSize(h)
        }
    })

    new Reaction(() => {
        let content = element.children[0]
        content.style.fill = element.HierarchyForegroundColor
    })


    WidthToStyle(element)
    HeightToStyle(element)
}
