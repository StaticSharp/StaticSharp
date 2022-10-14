
var capturedControl = undefined
function CaptureControlMove() {
    
    let eventClone = new event.constructor(event.type, event) 
    console.log(eventClone)
    //capturedControl.dispatchEvent(eventClone);
}

function UncaptureControl() {
    if (capturedControl) {
        document.removeEventListener("mousemove", CaptureControlMove)
        document.removeEventListener("mouseup", UncaptureControl)
    }
}

function CaptureControl(element) {
    if (capturedControl)
        UncaptureControl()

    capturedControl = element
    document.addEventListener(
        "mousemove",
        CaptureControlMove
    )

    document.addEventListener(
        "mouseup",
        UncaptureControl,
        { once: true }
    );
}


function Slider(element) {
    Block(element)

    element.style.touchAction = "none"

    element.Reactive = {
        Min: 0,
        Max: 1,
        Step: 0,
        Range: 0.2,

        Value: () => element.Min,
        ValueActual: () => element.Value,

        Height: 20,

        IsVertical: () => element.Width < element.Height,

        VerticalPaddingSum: () => Sum(element.PaddingTop, element.PaddingBottom, 0),
        HorizontalPaddingSum: () => Sum(element.PaddingLeft, element.PaddingRight, 0),
        TrackWidth: () => Sum(element.Width, -element.HorizontalPaddingSum),
        TrackHeight: () => Sum(element.Height, -element.VerticalPaddingSum),

        ThumbThickhess: () => element.IsVertical ? element.TrackWidth : element.TrackHeight,
        TrackLenght: () => element.IsVertical ? element.TrackHeight : element.TrackWidth,

        ThumbLenght: () => (element.TrackLenght - element.ThumbThickhess) * (element.Range / (element.Max - element.Min)) + element.ThumbThickhess,

        //Thumb: () => element.FirstChild
    }

    let thumb = Create(element, Block)
    thumb.Reactive = {
        BackgroundColor: new Color("#000"),
        Visibility: () => thumb.Hover ? 1: 0.5,
        Width: () => element.IsVertical ? element.ThumbThickhess : element.ThumbLenght,
        Height: () => element.IsVertical ? element.ThumbLenght : element.ThumbThickhess,
        Radius: () => Min(thumb.Width, thumb.Height) / 2
    }

    /*thumb.Events.TouchStart = () => {
        event.stopPropagation()
        event.preventDefault()
    }*/



    thumb.Events.PointerDown = () => {
        let startX = event.clientX - element.AbsoluteX
        let startY = event.clientY - element.AbsoluteY


        console.log("thumb PointerDown", startX, startY)
        event.stopPropagation()
        event.preventDefault()
        thumb.setPointerCapture(event.pointerId)

        //CaptureControl(thumb)
        thumb.Events.PointerMove = () => {
            let x = event.clientX - element.AbsoluteX
            let y = event.clientY - element.AbsoluteY
            console.log("thumb PointerMove", x - startX)
            //event.preventDefault()
            //event.stopPropagation()
            //let offset = 

        }
        thumb.Events.PointerUp = () => {
            thumb.Events.PointerUp = undefined
            thumb.Events.PointerMove = undefined
        }
        return false
    }




    /*new Reaction(() => {
        let thumb = element.Thumb
        if (thumb) {
            thumb.Width = () => element.IsVertical ? element.ThumbThickhess : element.ThumbLenght
            thumb.Height = () => element.IsVertical ? element.ThumbLenght : element.ThumbThickhess 

            
        }
    }) */


    


    /*let thumb = document.createElement("thumb")
    element.appendChild(thumb)
    thumb.style.backgroundColor = "red"

    new Reaction(() => {
        let thumbThickhess = element.IsVertical ?  Min(element.Width, element.Height)
        
    })*/


    /*new Reaction(() => {
        element.input = element.children[0]
        //element.input.value = element.Value

        element.input.oninput = function () {
            console.log("Slider.oninput", this.valueAsNumber)
            //let d = Reaction.beginDeferred()
            element.ValueActual = this.valueAsNumber
            //d.end()
        }
    })*/

    /*new Reaction(() => {
        element.input.style.width = element.Width+"px"
    })
    new Reaction(() => {
        element.input.style.height = element.Height + "px"
    })
    new Reaction(() => {
        element.input.min = element.Min
    })
    new Reaction(() => {
        element.input.max = element.Max
    })
    new Reaction(() => {
        element.input.step = element.Step <= 0 ? "any" : element.Step
    })
    new Reaction(() => {
        //console.log("element.input.value = element.Value", element.Value)
        element.input.value = element.Value
    })*/

}
