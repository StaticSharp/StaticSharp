function MaterialDesignIconInline(element) {
    Inline(element)
    MaterialDesignIcon(element)

    element.Reactive = {
        BaselineOffset: 0.14
    }

    let width = Number(element.dataset.width)
    let height = Number(element.dataset.height)

    let scale = Number(element.dataset.scale | 1)

    element.style.display = "inline-block"
    element.style.verticalAlign = "baseline"

    element.style.width = `calc(1em * ${scale * height/width})`//width + "px"
    element.style.height = scale+"em"
    element.style.overflow = "visible"
    //element.style.backgroundColor = "burlywood"

    new Reaction(() => {
        let content = element.children[0]
        content.style.position = "relative"
        let baselineOffset = element.BaselineOffset

        if (baselineOffset!=0)
            content.style.top = 100 * baselineOffset + "%"
        else
            content.style.top = ""
    })


}