StaticSharpClass("StaticSharp.SvgIconBlock", (element) => {
    StaticSharp.AspectBlock(element)
    StaticSharp.SvgIcon(element)


    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.content
        yield* element.UnmanagedChildren
    })

    new Reaction(() => {
        FitImage(
            element,
            element.content, element.NativeAspect,
            element.Fit, element.GravityVertical, element.GravityHorizontal
        )
    })


})
