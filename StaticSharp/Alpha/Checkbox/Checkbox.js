
function Checkbox(element) {
    Block(element)

    console.log("Checkbox")

    element.Reactive = {

        Value: false
        
    }

    new Reaction(() => {
        element.input = element.children[0]
        element.input.value = element.Value

        element.input.oninput = function () {
            let d = Reaction.beginDeferred()
            element.Value = this.valueAsNumber
            d.end()
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

    WidthToStyle(element)
    HeightToStyle(element)
}
