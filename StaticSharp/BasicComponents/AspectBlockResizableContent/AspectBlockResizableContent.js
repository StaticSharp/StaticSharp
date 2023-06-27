StaticSharpClass("StaticSharp.AspectBlockResizableContent", (element) => {
    StaticSharp.AspectBlock(element)

    let baseHtmlNodesOrdered = element.HtmlNodesOrdered
    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.content
        yield* baseHtmlNodesOrdered
    })


    new Reaction(() => {
        FitImage(
            element,
            element.content, element.NativeAspect,
            element.Fit, element.GravityVertical, element.GravityHorizontal
        )
    })
})