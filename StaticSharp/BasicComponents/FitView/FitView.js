
function FitView(element) {
    AspectBlock(element)
    element.isFitView = true

    element.Reactive = {
        NativeWidth: e => e.Child.Width,
        NativeHeight: e => e.Child.Height,
    }


    let underlay = document.createElement("underlay")
    CreateSocket(element, "Child", element)

    let baseHtmlNodesOrdered = element.HtmlNodesOrdered
    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield* baseHtmlNodesOrdered
        yield underlay
    })

    new Reaction(() => {
        SyncChildren(underlay, [element.Child])
    })

    new Reaction(() => {
        //console.log("element.NativeWidth", element.NativeWidth)
        let fit = element.Fit

        let contentWidth = element.NativeWidth
        let contentHeight = element.NativeHeight

        let containerWidth = element.Width
        let containerHeight = element.Height


        underlay.style.width = ToCssSize(contentWidth)
        underlay.style.height = ToCssSize(contentHeight)

        underlay.style.transformOrigin = "top left"
        let x = 0
        let y = 0
        let scaleX = 1
        let scaleY = 1
        scaleX = containerWidth / contentWidth
        scaleY = containerHeight / contentHeight

        if (fit == "Stretch") {

        } else {
            let gravityVertical = element.GravityVertical
            let gravityHorizontal = element.GravityHorizontal


            let realAspect = containerWidth / containerHeight
            //console.log("realAspect", realAspect, nativeAspect)
            
            /*let contentWidth = containerWidth
            let contentHeight = containerHeight*/

            let sign = (fit == "Inside") ? 1 : -1
            let nativeAspect = element.NativeAspect
            if (sign * realAspect < sign * nativeAspect) {
                scaleY = scaleX
                contentHeight = contentHeight * scaleY
                y = (containerHeight - contentHeight) * (0.5 * sign * gravityVertical + 0.5)
            } else {
                scaleX = scaleY
                contentWidth = contentWidth*scaleX
                x = (containerWidth - contentWidth) * (0.5 * sign * gravityHorizontal + 0.5)
            }

        }
        


        underlay.style.transform = ` translate(${x}px, ${y}px) scale(${scaleX}, ${scaleY})`;
        return

        //underlay.style.width = 


        if (fit == "Stretch") {

            underlay.style.width = "100%"
            underlay.style.height = "100%"



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



    })

}
