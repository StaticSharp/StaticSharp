
function SliderInitialization(element) {
    BlockInitialization(element)

    element.Reactive = {
        Min: 0,
        Max: 1,
        Step: 0,
        Value: ()=>element.Min,
        Height: 20
    }
}

function SliderBefore(element) {
    BlockBefore(element)

    WidthToStyle(element)
    HeightToStyle(element)


}

function SliderAfter(element) {
    BlockAfter(element)

    let input = element.children[0]
    input.value = element.Value

    input.oninput = function () {
        let d = Reaction.beginDeferred()
        element.Value = this.valueAsNumber
        d.end()
    }

    new Reaction(() => {
        input.style.width = element.Width+"px"
    })
    new Reaction(() => {
        input.style.height = element.Height + "px"
    })
    new Reaction(() => {
        input.min = element.Min
    })
    new Reaction(() => {
        input.max = element.Max
    })
    new Reaction(() => {
        input.step = element.Step <= 0 ? "any" : element.Step
    })
}