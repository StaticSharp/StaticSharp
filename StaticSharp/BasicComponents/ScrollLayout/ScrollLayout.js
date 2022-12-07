
function Thumb(element) {
    Block(element)
    element.Reactive = {
        BackgroundColor: () => element.Parent.HierarchyForegroundColor,
        
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
        InternalWidth: () => Sum(element.Content.InternalWidth, element.MarginLeft, element.MarginRight),
        InternalHeight: () => Sum(element.Content.InternalHeight, element.TopOffset, element.BottomOffset),
        //Content: () => element.Child("Content"),

        /*MarginLeft: () => (element.PaddingLeft!=undefined) ? 0 : element.Content.MarginLeft,
        MarginTop: () => (element.PaddingTop != undefined) ? 0 : element.Content.MarginTop,
        MarginRight: () => (element.PaddingRight != undefined) ? 0 : element.Content.MarginRight,
        MarginBottom: () => (element.PaddingBottom != undefined) ? 0 : element.Content.MarginBottom,*/

        LeftOffset: () => CalcOffset(element, element.Content, "Left"),
        RightOffset: () => CalcOffset(element, element.Content, "Right"),
        TopOffset: () => CalcOffset(element, element.Content, "Top"),
        BottomOffset: () => CalcOffset(element, element.Content, "Bottom"),

        ContentAreaWidth: () => element.Width - element.LeftOffset - element.RightOffset,
        ContentAreaHeight: () => element.Height - element.TopOffset - element.BottomOffset,

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
        ThumbSizeScale: () => element.ContentAreaHeight / element.Content.InternalHeight, 
        X: () => element.Width - verticalThumb.Width - element.ScrollBarMargin,
        Y: () => element.ScrollBarMargin + element.ScrollYActual * verticalThumb.ThumbPositionScale,
        Width: () => element.ScrollBarThickness,        
        Height: () => verticalThumb.ThumbTravel * verticalThumb.ThumbSizeScale,
    }
    

    let horizontalThumb = Create(element, Thumb)
    horizontalThumb.Reactive = {
        ThumbTravel: () => element.Width - 2 * element.ScrollBarMargin,
        ThumbPositionScale: () => horizontalThumb.ThumbTravel / element.Content.Width,
        ThumbSizeScale: () => element.ContentAreaWidth / element.Content.Width,        
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


    let scrollable = document.createElement("scrollable")
    element.appendChild(scrollable)

    scrollable.style.touchAction = "manipulation"
    scrollable.style.overflow = "auto"
    //scrollable.style.outline = "0.1px solid #f00"
    
    new Reaction(() => {
        scrollable.style.left = ToCssSize(element.LeftOffset)
        scrollable.style.top = ToCssSize(element.TopOffset)
        scrollable.style.width = ToCssSize(element.ContentAreaWidth)
        scrollable.style.height = ToCssSize(element.ContentAreaHeight)
    })



    scrollable.Events.Scroll = () => {
        let d = Reaction.beginDeferred()
        element.ScrollXActual = scrollable.scrollLeft
        element.ScrollYActual = scrollable.scrollTop
        d.end()
    }


    new Reaction(() => {
        scrollable.appendChild(element.Content)
        element.Content.LayoutWidth = () => Max(element.Content.InternalWidth, element.ContentAreaWidth)
        element.Content.LayoutHeight = () => Max(element.Content.InternalHeight, element.ContentAreaHeight)
    })


   /*OnChanged(
        () => element.Content,
        (p, c) => {
            if (c) {                
                
            }
        }
    )*/


}