function XToStyle(element) {
    return new Reaction(() => {
        element.style.top = (!!element.Y) ? element.Y + "px" : ""
    })
}
function YToStyle(element) {
    return new Reaction(() => {
        element.style.left = (!!element.X) ? element.X + "px" : ""
    })
}

function WidthToStyle(element) {
    return new Reaction(() => {
        element.style.width = (!!element.Width) ? element.Width + "px" : ""
    })
}

function HeightToStyle(element) {
    return new Reaction(() => {
        element.style.height = (!!element.Height) ? element.Height + "px" : ""
    })
}