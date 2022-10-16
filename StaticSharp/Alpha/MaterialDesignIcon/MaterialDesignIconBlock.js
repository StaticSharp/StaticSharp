

function MaterialDesignIconBlock(element) {
    Block(element)
    MaterialDesignIcon(element)


    let width = Number(element.dataset.width)
    let height = Number(element.dataset.height)


    element.Reactive = {

        Aspect: width / height,

        VerticalPaddingSum: () => Sum(element.PaddingTop, element.PaddingBottom, 0),
        HorizontalPaddingSum: () => Sum(element.PaddingLeft, element.PaddingRight, 0),

        InternalWidth: () => First((element.Height - element.VerticalPaddingSum) * element.Aspect + element.HorizontalPaddingSum, Sum(width, element.HorizontalPaddingSum)),
        InternalHeight: () => First((element.Width - element.HorizontalPaddingSum) / element.Aspect + element.VerticalPaddingSum, Sum(height, element.VerticalPaddingSum)),
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
            content.style.left = ToCssSize(element.PaddingLeft)
            content.style.top = ToCssSize(element.PaddingTop)
            content.style.width = ToCssSize(w)
            content.style.height = ToCssSize(h)
        }
    })

    new Reaction(() => {
        let content = element.children[0]
        content.style.fill = element.HierarchyForegroundColor
    })


    
}
