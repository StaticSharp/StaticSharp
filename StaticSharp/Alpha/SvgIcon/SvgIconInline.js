function SvgIconInline(element) {
    Inline(element)
    SvgIcon(element)

    element.Reactive = {
        BaselineOffset: 0.14
    }

    let width = Number(element.dataset.width)
    let height = Number(element.dataset.height)

    let scale = Number(element.dataset.scale || 1)

    element.style.display = "inline-block"
    element.style.verticalAlign = "baseline"

    element.style.width = `${scale * height / width}em`//width + "px"
    element.style.height = "1em"

    element.style.overflow = "visible"
    //element.style.backgroundColor = "burlywood"


    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.content
        yield* element.Children
    })

    element.AfterChildren = () => {
        let content = element.content
        content.style.display = "block"
        content.style.position = "relative"
        content.style.height = scale + "em"

        
    }


    new Reaction(() => {
        let content = element.content
        let baselineOffset = element.BaselineOffset

        content.style.top = `${1 - (1-baselineOffset)*scale}em`
    })


}