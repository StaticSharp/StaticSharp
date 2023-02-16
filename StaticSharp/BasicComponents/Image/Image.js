
function Image(element) {
    AspectBlock(element)



    /*new Reaction(() => {
        let content = element.children[0]
        let thumbnail = content.querySelector("#thumbnail")
        if (thumbnail) {
            thumbnail.style.display = "block"
        }
    })*/

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.content
        yield* element.Children
    })

    element.AfterChildren = function () {
        let thumbnail = element.content.querySelector("#thumbnail")
        if (thumbnail) {
            thumbnail.style.display = "block"
        }
    }

    
}
