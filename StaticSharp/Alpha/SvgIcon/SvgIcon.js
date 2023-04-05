function SvgIcon(element) {


    element.Reactive = {
        StrokeColor: undefined,
        StrokeWidth : undefined,
    }

    new Reaction(() => {
        element.children[0].style.fill = element.HierarchyForegroundColor
    })

    new Reaction(() => {
        if (element.StrokeColor != undefined)
            element.children[0].style.stroke = element.StrokeColor
    })

    new Reaction(() => {
        if (element.StrokeWidth != undefined)
            element.children[0].style.strokeWidth = ToCssSize(element.StrokeWidth)
    })
}