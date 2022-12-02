
function MaterialShadow(element) {
    Hierarchical(element)

    element.isBoxShadow = true

    element.Reactive = {
        Elevation: 10

    }

    let firstBoxShadow = true

    for (let i of element.Parent.Children) {
        if (i != element) {
            if (i.isBoxShadow) {
                firstBoxShadow = false
                break;
            }  
        }
    }

    function ambientShadow(e) {
        return `0px ${0.25 * e}px ${2 * e}px ${-0.5 * e}px rgba(0, 0, 0, 0.5)`
    }
    function hardShadow(e) {
        let spread = 0.15
        let x = 0.4
        let y = 0.8
        return `${x * e}px ${y * e}px ${2*spread * e}px ${-spread*e}px rgba(0, 0, 0, 0.2)`
    }

    function shadow(e) {
        return ambientShadow(e) + "," + hardShadow(e)
    }


    if (firstBoxShadow) {
        new Reaction(() => {
            element.Parent.style.boxShadow = shadow(element.Elevation)
        })
    } else {
        new Reaction(() => {
            element.style.boxShadow = shadow(element.Elevation)
        })
        new Reaction(() => {
            element.style.width = ToCssSize(element.Parent.Width)
            element.style.height = ToCssSize(element.Parent.Height)
        })

        new Reaction(() => {
            element.style.borderTopLeftRadius = ToCssSize(element.Parent.RadiusTopLeft)
            element.style.borderTopRightRadius = ToCssSize(element.Parent.RadiusTopRight)
            element.style.borderBottomLeftRadius = ToCssSize(element.Parent.RadiusBottomLeft)
            element.style.borderBottomRightRadius = ToCssSize(element.Parent.RadiusBottomRight)
        })
    }

}