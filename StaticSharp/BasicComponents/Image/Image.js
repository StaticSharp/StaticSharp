
function Image(element) {
    AspectBlock(element)



    new Reaction(() => {
        let content = element.children[0]
        let thumbnail = content.querySelector("#thumbnail")
        if (thumbnail) {
            thumbnail.style.display = "block"
        }
    })

    
}
