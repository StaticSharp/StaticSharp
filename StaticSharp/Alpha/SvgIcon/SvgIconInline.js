function SvgIconInline(element) {
    Inline(element)
    SvgIcon(element)

    element.Reactive = {
        BaselineOffset: 0.14
    }

    let content = element.children[0]
    /*content.style.display = "block"
    content.style.position = "relative"
    content.style.transform = `scale(1,0.5)`
    content.style.transformOrigin = "top";*/

    new Reaction(() => {
        let baselineOffset = element.BaselineOffset
        //element.style.transform = `scale(1,2)`

        //element.style.transform = `translate(0, ${100*baselineOffset + "%"})`

        //content.style.top = `${1 - (1 - baselineOffset) * element.scale}em`
        //console.log("element.scale", element.scale)
        //element.style.height = `${element.scale}em`
    })


}