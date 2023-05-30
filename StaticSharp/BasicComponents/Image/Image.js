
function Image(element) {
    AspectBlock(element)

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

    /*new Reaction(() => {
        let content = element.children[0]
        let thumbnail = content.querySelector("#thumbnail")
        if (thumbnail) {
            thumbnail.style.display = "block"
        }
    })*/

    

    /*element.AfterChildren = function () {
        let thumbnail = element.content.querySelector("#thumbnail")
        if (thumbnail) {
            thumbnail.style.display = "block"
        }
    }*/

    
}
