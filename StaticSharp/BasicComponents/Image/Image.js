
function Image(element) {
    AspectBlock(element)



    /*new Reaction(() => {
        let content = element.children[0]
        let thumbnail = content.querySelector("#thumbnail")
        if (thumbnail) {
            thumbnail.style.display = "block"
        }
    })*/

    

    element.AfterChildren = function () {
        let thumbnail = element.content.querySelector("#thumbnail")
        if (thumbnail) {
            thumbnail.style.display = "block"
        }
    }

    
}
