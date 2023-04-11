
function BaseModifier(element) {
    Hierarchical(element)

    //TODO: fix file protocol
    /*if (element.parentElement.tagName == "A") {
        if (window.location.protocol == "file:") {
            let a = element.parentElement
            let href = a.getAttribute("href")
            let AbsoluteUrlRegExp = new RegExp('^(?:[a-z+]+:)?//', 'i')
            if (!AbsoluteUrlRegExp.test(href)) {
                a.setAttribute("href", href + ".html")
            }
        }        
    }*/
    


    //var extension = (window.location.protocol == "file:") ? ".html" : ""
    

    //window.location.replace(matchLanguage([{{ languages }}]) + extension)

    /**type */

    element.isModifier = true
    let baseAs = element.as
    element.as = function (typeName) {
        let result = baseAs(typeName)
        if (result != undefined)
            return result

        if (element.Modifiers != undefined) {
            let oldAs = element.as
            element.as = () => undefined
            result = element.Modifiers.First(x => x.is(typeName),()=>undefined)
            element.as = oldAs
        }
        
        return result
    }




    element.Reactive = {
        Hover: false,
        Selectable: undefined,

        BackgroundColor: undefined,
        HierarchyBackgroundColor: () => element.BackgroundColor || element.Parent.HierarchyBackgroundColor,

        ForegroundColor: () => { 
            if (element.BackgroundColor != undefined)
                return element.BackgroundColor.ContrastColor()
            else
                return undefined
        },
        HierarchyForegroundColor: () => {
            let result = element.ForegroundColor
            if (result) return result
            let parent = element.Parent
            if (parent) {
                result = parent.HierarchyForegroundColor
                if (result) return result
            }
            return new Color(0)
        },

        TextDecorationColor: () => {
            if (element.ForegroundColor != undefined)
                return element.ForegroundColor
            else
                return undefined
        },

        Modifiers: Enumerable.Empty(),


        FontSize: undefined,
        HierarchyFontSize: () => element.FontSize || element.Parent.HierarchyFontSize,

        Radius: undefined,
        RadiusTopLeft: () => element.Radius,
        RadiusTopRight: () => element.Radius,
        RadiusBottomLeft: () => element.Radius,
        RadiusBottomRight: () => element.Radius,

        Visibility: 1
    }

    new Reaction(() => {
        let visibility = element.Visibility
        if (visibility == 0) {
            element.style.visibility = "hidden"
            element.style.opacity = ""
        } else if (visibility == 1) {
            element.style.visibility = ""
            element.style.opacity = ""
        } else {
            element.style.opacity = visibility
            element.style.visibility = ""
        }        
    })


    new Reaction(() => {
        element.style.borderTopLeftRadius       = ToCssSize(element.RadiusTopLeft)
        element.style.borderTopRightRadius      = ToCssSize(element.RadiusTopRight)
        element.style.borderBottomLeftRadius    = ToCssSize(element.RadiusBottomLeft)
        element.style.borderBottomRightRadius   = ToCssSize(element.RadiusBottomRight)
    })



    new Reaction(() => {
        let selectable = element.Selectable
        if (selectable!=undefined)
            element.style.userSelect = element.Selectable ? "text" : "none"
        else
            element.style.userSelect = ""
    })
    

    new Reaction(() => {
        element.style.backgroundColor = ToCssValue(element.BackgroundColor)
    })

    new Reaction(() => {
        element.style.color = ToCssValue(element.ForegroundColor)
    })

    new Reaction(() => {
        element.style.textDecorationColor = ToCssValue(element.TextDecorationColor)
    })
}
