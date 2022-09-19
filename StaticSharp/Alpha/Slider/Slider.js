
function Slider(element) {
    Block(element)

    element.Reactive = {
        Min: 0,
        Max: 1,
        Step: 0,

        Value: () => element.Min,
        ValueActual: () => element.Value,

        Height: 20,
        
    }

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

    new Reaction(() => {
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
    })

    WidthToStyle(element)
    HeightToStyle(element)
}
