function ToCssSize(value) {
    return (value!=undefined) ? value.toFixed(4) + "px" : ""
}

function ToCssValue(value) {
    return (value != undefined) ? value : ""
}

function XToStyle(element) {
    return new Reaction(() => {
        let value = element.X
        /*if (isNaN(value)) {
            console.error(element.tagName + ".X is NaN", element)
        }*/
        element.style.left = ToCssSize(value)
    })
}
function YToStyle(element) {
    return new Reaction(() => {
        let value = element.Y
        if (isNaN(value)) {
            console.error(element.tagName + ".Y is NaN", element)
            //throw new Error(element.tagName +".Width is NaN")
        }
        element.style.top = ToCssSize(value)
    })
}

function WidthToStyle(element) {
    return new Reaction(() => {
        let value = element.Width
        if (isNaN(value)) {
            console.error(element.tagName + ".Width is NaN", element)
            //throw new Error(element.tagName +".Width is NaN")
        }
        element.style.width = ToCssSize(value)
    })
}

function HeightToStyle(element) {    
    return new Reaction(() => {
        let value = element.Height
        if (isNaN(value)) {
            console.error(element.tagName + ".Height is NaN", element)
            //throw new Error(element.tagName+".Height is NaN")
        }
        element.style.height = ToCssSize(value)
    })
}

