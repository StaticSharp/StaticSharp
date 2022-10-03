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
        console.log("------------", Sum(element.dataset.width, HorizontalPaddingSum))
    })


    new Reaction(() => {
        let content = element.children[0]
        content.style.width = ToCssSize(element.InternalWidth - element.HorizontalPaddingSum)
        content.style.height = ToCssSize(element.InternalHeight - element.VerticalPaddingSum)
    })

    new Reaction(() => {
        let content = element.children[0]
        content.style.fill = element.HierarchyForegroundColor
    })


    WidthToStyle(element)
    HeightToStyle(element)
}
