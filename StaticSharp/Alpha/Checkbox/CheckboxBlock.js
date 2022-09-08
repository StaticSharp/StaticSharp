
function Checkbox(element) {
    Block(element)

    new Reaction(() => {
        element.input.style.width = element.Width + "px"
    })
    new Reaction(() => {
        element.input.style.height = element.Height + "px"
    })


    WidthToStyle(element)
    HeightToStyle(element)
}
