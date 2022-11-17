function ToCssSize(value) {
    return (value!=undefined) ? value + "px" : ""
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



function Try(func, defaultValue, exceptions) {
    //console.log("try", func, defaultValue, exceptions)
    try {
        return func()
    } catch (e) {
        console.warn(e)
        if (exceptions == undefined) {
            return defaultValue
        } else {
            for (let i of exceptions) {
                if (e instanceof i) {
                    return defaultValue
                }
            }
            throw e
        }
    }
}