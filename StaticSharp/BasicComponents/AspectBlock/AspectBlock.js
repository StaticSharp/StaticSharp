function AspectBlock(element) {
    Block(element)

    let nativeWidth = Number(element.dataset.width)
    let nativeHeight = Number(element.dataset.height)

    let nativeAspect = nativeWidth / nativeHeight

    element.Reactive = {

        Aspect: nativeAspect,
        /*InternalWidth*/ Width: () => First(element.Height * element.Aspect, nativeWidth),
        /*InternalHeight*/ Height: () => First(element.Width / element.Aspect, nativeHeight),

        Fit: "Inside",
        GravityVertical: 0,
        GravityHorizontal: 0,
    }




    new Reaction(() => {
        let content = element.children[0]

        if (element.Fit == "Stretch") {

            content.style.width = "100%"
            content.style.height = "100%"

        } else {
            let realAspect = element.Width / element.Height

            let x = 0
            let y = 0
            let contentWidth = element.Width
            let contentHeight = element.Height

            

            let sign = (element.Fit == "Inside") ? 1 : -1

            if (sign * realAspect < sign * nativeAspect) {
                contentHeight = contentWidth / nativeAspect
                y = (element.Height - contentHeight) * (0.5 * sign * element.GravityVertical + 0.5)
            } else {
                contentWidth = contentHeight * nativeAspect
                x = (element.Width - contentWidth) * (0.5 * sign * element.GravityHorizontal + 0.5)
            }

            content.style.top = ToCssSize(y)
            content.style.left = ToCssSize(x)
            content.style.width = ToCssSize(contentWidth)
            content.style.height = ToCssSize(contentHeight)

            content.style.clipPath = GetClipRect(element, x, y, contentWidth, contentHeight)

        }

    })


}