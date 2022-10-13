
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
        BackgroundColor: new Color("#555"),
        Visibility: () => thumb.Hover ? 0.5: 0.25,
        Width: () => element.IsVertical ? element.ThumbThickhess : element.ThumbLenght,
        Height: () => element.IsVertical ? element.ThumbLenght : element.ThumbThickhess,
        Radius: () => Min(thumb.Width, thumb.Height) / 2
    }

    thumb.Events.PointerDown = () => {
        let startX = event.clientX
        console.log("PointerDown", startX, event.pointerId)
        event.stopPropagation()
        thumb.setPointerCapture(event.pointerId)

        //CaptureControl(thumb)
        thumb.Events.PointerMove = () => {
            let x = event.clientX
            console.log("PointerMove", x - startX)
            event.preventDefault()
        }
        thumb.Events.MouseUp = () => {
            thumb.Events.MouseMove = undefined
        }
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


    new Reaction(() => {
        element.input = element.children[0]
        //element.input.value = element.Value

        element.input.oninput = function () {
            console.log("Slider.oninput", this.valueAsNumber)
            //let d = Reaction.beginDeferred()
            element.ValueActual = this.valueAsNumber
            //d.end()
        }
    })

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
