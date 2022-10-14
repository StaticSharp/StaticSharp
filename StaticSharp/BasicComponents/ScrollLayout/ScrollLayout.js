
function Thumb(element) {
    Block(element)
    element.Reactive = {
        BackgroundColor: new Color("#000"),
        
        Radius: () => Min(element.Width, element.Height) / 2,
        Depth: 1,
        Visibility: () => (window.Touch || element.ThumbSizeScale >= 1) ? 0 : element.Hover ? 0.5 : 0.25,
    }
}

function SetupPointerDrag(element, func) {
    element.Events.PointerDown = () => {
        event.stopPropagation()
        event.preventDefault()
        element.setPointerCapture(event.pointerId)

        element.Events.PointerMove = () => {
            let x = event.movementX
            let y = event.movementY
            if (x != 0 || y != 0) {
                func(x,y)
            }            
        }
        element.Events.PointerUp = () => {
            element.Events.PointerUp = undefined
            element.Events.PointerMove = undefined
        }
        return false
    }
}



function ScrollLayout(element) {
    Block(element)

    


    element.Reactive = {
        InternalWidth: () => element.Content.InternalWidth,
        InternalHeight: () => element.Content.InternalHeight,
        Content: () => element.Child("Content"),

        ScrollX: undefined,
        ScrollY: undefined,

        ScrollXActual: 0,
        ScrollYActual: 0,

        ScrollBarThickness: 4,
        ScrollBarMargin: 2,

        ShowScrollBars: !window.Touch
    }

    

    let verticalThumb = Create(element, Thumb)
    verticalThumb.Reactive = {
        ThumbTravel: () => element.Height - 2 * element.ScrollBarMargin,
        ThumbPositionScale: () => verticalThumb.ThumbTravel / element.Content.InternalHeight,
        ThumbSizeScale: () => element.Height / element.Content.InternalHeight, 
        X: () => element.Width - verticalThumb.Width - element.ScrollBarMargin,
        Y: () => element.ScrollBarMargin + element.ScrollYActual * verticalThumb.ThumbPositionScale,
        Width: () => element.ScrollBarThickness,        
        Height: () => verticalThumb.ThumbTravel * verticalThumb.ThumbSizeScale,
    }

    /*verticalThumb.Events.MouseDown = () => {
        console.log("mouseDown")
    }*/

    

    let horizontalThumb = Create(element, Thumb)
    horizontalThumb.Reactive = {
        ThumbTravel: () => element.Width - 2 * element.ScrollBarMargin,
        ThumbPositionScale: () => horizontalThumb.ThumbTravel / element.Content.Width,
        ThumbSizeScale: () => element.Width / element.Content.Width,        
        X: () => element.ScrollBarMargin + element.ScrollXActual * horizontalThumb.ThumbPositionScale,
        Y: () => element.Height - horizontalThumb.Height - element.ScrollBarMargin,
        Width: () => horizontalThumb.ThumbTravel * horizontalThumb.ThumbSizeScale,
        Height: () => element.ScrollBarThickness,        
    }

    SetupPointerDrag(verticalThumb, (x, y) => {
        scrollable.scrollTop += y / verticalThumb.ThumbPositionScale
    })

    SetupPointerDrag(horizontalThumb, (x, y) => {
        scrollable.scrollLeft += x / horizontalThumb.ThumbPositionScale
    })


    //element.style.overflow = "hidden"

    let scrollable = document.createElement("scrollable")
    element.appendChild(scrollable)
    scrollable.style.touchAction = "manipulation"
    scrollable.style.overflow = "auto"
    scrollable.style.outline = "0.1px solid #f00"
    scrollable.style.width = "100%"
    scrollable.style.height = "100%"

    scrollable.Events.Scroll = () => {
        //console.log(scrollable.scrollLeft, scrollable.scrollTop, verticalThumb.ThumbTravel, element.Height)
        let d = Reaction.beginDeferred()
        element.ScrollXActual = scrollable.scrollLeft
        element.ScrollYActual = scrollable.scrollTop
        d.end()
    }

    /*scrolllayout {
    touch-action: manipulation;
    overflow-x: scroll;
    overflow-y: scroll;
}
*/

    /*new Reaction(() => {
        console.log("scroll", element.ScrollXActual)
    })*/

    OnChanged(
        () => element.Content,
        (p, c) => {
            if (c) {

                
                scrollable.appendChild(c)
                


                //console.log(c.InternalWidth,c)
                c.LayoutWidth = () => Max(c.InternalWidth, element.Width)
                c.LayoutHeight = () => Max(c.InternalHeight, element.Height)
            }

            /*if (p) {
                p.LayoutWidth = undefined
                p.LayoutHeight = undefined
            }*/
        }
    )


}