function AspectBlock(element) {
    Block(element)

    element.Reactive = {

        NativeWidth: undefined,
        NativeHeight: undefined,
        NativeAspect: e => e.NativeWidth / e.NativeHeight,

        Aspect: e => e.NativeAspect,
        Width: () => First(element.Height * element.Aspect, element.NativeWidth),
        Height: () => First(element.Width / element.Aspect, element.NativeHeight),

        Fit: "Inside",
        GravityVertical: 0,
        GravityHorizontal: 0,
    }

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.content
        yield* element.UnmanagedChildren
    })


    new Reaction(() => {
        let content = element.content

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
            let nativeAspect = element.NativeAspect
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