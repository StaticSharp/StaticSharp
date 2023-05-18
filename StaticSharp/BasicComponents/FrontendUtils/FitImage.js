function FitImage(container, imageTag, nativeAspect, fit, gravityVertical, gravityHorizontal) {


    if (fit == "Stretch") {

        imageTag.style.width = "100%"
        imageTag.style.height = "100%"

        //TODO:
        //imageTag.style.clipPath = GetClipRect(container, x, y, contentWidth, contentHeight)
    } else {
        let containerWidth = container.Width
        let containerHeight = container.Height

        let realAspect = containerWidth / containerHeight

        let x = 0
        let y = 0
        let contentWidth = containerWidth
        let contentHeight = containerHeight



        let sign = (fit == "Inside") ? 1 : -1
        //let nativeAspect = element.NativeAspect
        if (sign * realAspect < sign * nativeAspect) {
            contentHeight = contentWidth / nativeAspect
            y = (containerHeight - contentHeight) * (0.5 * sign * gravityVertical + 0.5)
        } else {
            contentWidth = contentHeight * nativeAspect
            x = (containerWidth - contentWidth) * (0.5 * sign * gravityHorizontal + 0.5)
        }

        imageTag.style.top = ToCssSize(y)
        imageTag.style.left = ToCssSize(x)
        imageTag.style.width = ToCssSize(contentWidth)
        imageTag.style.height = ToCssSize(contentHeight)
        imageTag.style.clipPath = GetClipRect(container, x, y, contentWidth, contentHeight)

    }
}