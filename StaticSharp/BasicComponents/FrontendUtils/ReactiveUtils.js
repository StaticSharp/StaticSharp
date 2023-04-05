function ToCssSize(value) {
    return (value!=undefined) ? value.toFixed(4) + "px" : ""
}

function ToCssValue(value) {
    return (value != undefined) ? value : ""
}

function DepthToStyle(element) {
    return new Reaction(() => {
        element.style.zIndex = element.Depth
    })
}


function XToStyle(element) {
    return new Reaction(() => {
        element.style.left = ToCssSize(element.X)
    })
}
function YToStyle(element) {
    return new Reaction(() => {
        element.style.top = ToCssSize(element.Y)
    })
}

function WidthToStyle(element) {
    return new Reaction(() => {
        element.style.width = ToCssSize(element.Width)
    })
}

function HeightToStyle(element) {
    return new Reaction(() => {
        element.style.height = ToCssSize(element.Height)
    })
}

