function ScrollLayout(element) {
    Block(element)

    element.Reactive = {
        InternalWidth: () => element.Content.InternalWidth,
        InternalHeight: () => element.Content.InternalHeight,
        Content: () => element.Child("Content")
    }

    element.style.overflow = "auto"
    element.style.outline = "0.1px solid #f00"

    Create(element, Slider).Reactive = {
        Depth: 1,
        Width: () => element.Width,
        Height : 8,
    }
    /*new Reaction(() => {
        console.log("scroll", element.ScrollXActual)
    })*/

    OnChanged(
        () => element.Content,
        (p, c) => {
            if (c) {
                //console.log(c.InternalWidth,c)
                c.LayoutWidth = () => Max(c.InternalWidth, element.Width)
                c.LayoutHeight = () => Max(c.InternalHeight, element.Height)
            }

            if (p) {
                p.LayoutWidth = undefined
                p.LayoutHeight = undefined
            }
        }
    )


}